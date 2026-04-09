using CreditSystem.Application.Interfaces;
using CreditSystem.Application.UseCases.ProcessRiskEvaluated.Commands;
using CreditSystem.Domain.Entities;
using CreditSystem.Domain.Enums;
using CreditSystem.Domain.Events;
using CreditSystem.Domain.Interfaces;
using CreditSystem.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CreditSystem.Application.Tests.UseCases.ProcessRiskEvaluated;

[TestFixture]
public class ProcessRiskEvaluatedCommandHandlerTests
{
    private Mock<ILogger<ProcessRiskEvaluatedCommandHandler>> _logger = null!;
    private Mock<ICreditRequestRepository> _repository = null!;
    private Mock<IConditionsService> _conditionsService = null!;
    private Mock<IEventBus> _eventBus = null!;
    private ProcessRiskEvaluatedCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _logger = new Mock<ILogger<ProcessRiskEvaluatedCommandHandler>>();
        _repository = new Mock<ICreditRequestRepository>();
        _conditionsService = new Mock<IConditionsService>();
        _eventBus = new Mock<IEventBus>();
        _eventBus.Setup(e => e.PublishAsync(It.IsAny<object>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _handler = new ProcessRiskEvaluatedCommandHandler(_logger.Object, _repository.Object, _conditionsService.Object, _eventBus.Object);
    }

    [Test]
    public async Task Handle_when_conditions_calculated_updates_status_and_publishes_ConditionsCalculated()
    {
        var requestId = Guid.NewGuid();
        var entity = CreditRequest.Create(requestId, "client-1", 5_000m);
        entity.SetStatus(CreditRequestStatus.RiskEvaluated);
        var conditions = new CreditConditions(0.08m, 24, 250m, 6_000m);
        _repository.Setup(r => r.GetByIdAsync(requestId, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _repository.Setup(r => r.UpdateAsync(It.IsAny<CreditRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _conditionsService.Setup(s => s.CalculateConditionsAsync(requestId, 5_000m, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conditions);

        var command = new ProcessRiskEvaluatedCommand(requestId, DateTimeOffset.UtcNow);
        await _handler.Handle(command, CancellationToken.None);

        _repository.Verify(r => r.UpdateAsync(It.Is<CreditRequest>(e => e.Status == CreditRequestStatus.ConditionsCalculated && e.Conditions != null), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        _eventBus.Verify(e => e.PublishAsync(It.Is<ConditionsCalculated>(ev =>
            ev.RequestId == requestId && ev.InterestRate == 0.08m && ev.TermMonths == 24), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_when_request_not_found_does_not_call_service_nor_publish()
    {
        _repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((CreditRequest?)null);

        var command = new ProcessRiskEvaluatedCommand(Guid.NewGuid(), DateTimeOffset.UtcNow);
        await _handler.Handle(command, CancellationToken.None);

        _conditionsService.Verify(s => s.CalculateConditionsAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()), Times.Never);
        _eventBus.Verify(e => e.PublishAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task Handle_when_conditions_throws_sets_Failed_and_publishes_StepFailed()
    {
        var requestId = Guid.NewGuid();
        var entity = CreditRequest.Create(requestId, "c", 1m);
        entity.SetStatus(CreditRequestStatus.RiskEvaluated);
        _repository.Setup(r => r.GetByIdAsync(requestId, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _repository.Setup(r => r.UpdateAsync(It.IsAny<CreditRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _conditionsService.Setup(s => s.CalculateConditionsAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Conditions error"));

        var command = new ProcessRiskEvaluatedCommand(requestId, DateTimeOffset.UtcNow);
        await _handler.Handle(command, CancellationToken.None);

        _eventBus.Verify(e => e.PublishAsync(It.Is<StepFailed>(ev => ev.RequestId == requestId && ev.Step == "Conditions"), It.IsAny<CancellationToken>()), Times.Once);
    }
}
