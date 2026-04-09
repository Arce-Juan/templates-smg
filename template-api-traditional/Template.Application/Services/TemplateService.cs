using AutoMapper;
using System.Net;
using Template.Application.Common;
using Template.Application.Interfaces;
using Template.Application.Interfaces.Repositories;
using Template.Application.ValueObjects;
using Template.Domain.Entities;

namespace Template.Application.Services;

public class TemplateService : ITemplateService
{
    private readonly ITemplateRepository _templateRepository;
    private readonly IMapper _mapper;

    public TemplateService(ITemplateRepository templateRepository, IMapper mapper)
    {
        _templateRepository = templateRepository;
        _mapper = mapper;
    }

    public async Task<Response<TemplateDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _templateRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                return Response<TemplateDto>.Failure("Template not found", statusCode: HttpStatusCode.NotFound);
            }

            var dto = _mapper.Map<TemplateDto>(entity);
            return Response<TemplateDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return Response<TemplateDto>.Failure($"Error retrieving template: {ex.Message}", statusCode: HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<IReadOnlyList<TemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = await _templateRepository.GetAllAsync(cancellationToken);
            var dtos = _mapper.Map<IReadOnlyList<TemplateDto>>(entities);
            return Response<IReadOnlyList<TemplateDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Response<IReadOnlyList<TemplateDto>>.Failure($"Error retrieving templates: {ex.Message}", statusCode: HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<TemplateDto>> CreateAsync(TemplateDto template, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = _mapper.Map<TemplateEntity>(template);
            var createdEntity = await _templateRepository.CreateAsync(entity, cancellationToken);
            var dto = _mapper.Map<TemplateDto>(createdEntity);
            
            return Response<TemplateDto>.Ok(dto, "Template created successfully");
        }
        catch (Exception ex)
        {
            return Response<TemplateDto>.Failure($"Error creating template: {ex.Message}", statusCode: HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<TemplateDto>> UpdateAsync(Guid id, TemplateDto template, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingEntity = await _templateRepository.GetByIdAsync(id, cancellationToken);
            if (existingEntity == null)
            {
                return Response<TemplateDto>.Failure("Template not found", statusCode: HttpStatusCode.NotFound);
            }

            _mapper.Map(template, existingEntity);
            await _templateRepository.UpdateAsync(existingEntity, cancellationToken);
            
            var dto = _mapper.Map<TemplateDto>(existingEntity);
            return Response<TemplateDto>.Ok(dto, "Template updated successfully");
        }
        catch (Exception ex)
        {
            return Response<TemplateDto>.Failure($"Error updating template: {ex.Message}", statusCode: HttpStatusCode.InternalServerError);
        }
    }
}
