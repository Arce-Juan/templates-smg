using Template.Domain.Enums;
using Stateless;

namespace Template.Domain.StateMachine;

public static class TemplateStateMachine
{
    public static async Task<bool> CanTransitionToAsync(TemplateStatus currentState, TemplateStatus newState, CancellationToken cancellationToken = default)
    {
        if (currentState == newState)
            return false;
        var machine = CreateConfiguredMachine(currentState);
        var permitted = await machine.GetPermittedTriggersAsync(cancellationToken).ConfigureAwait(false);
        return permitted.Contains(newState);
    }

    public static bool CanTransitionTo(TemplateStatus currentState, TemplateStatus newState)
    {
        if (currentState == newState)
            return false;
        var machine = CreateConfiguredMachine(currentState);
        return machine.GetPermittedTriggers().Contains(newState);
    }

    private static StateMachine<TemplateStatus, TemplateStatus> CreateConfiguredMachine(TemplateStatus initialState)
    {
        var machine = new StateMachine<TemplateStatus, TemplateStatus>(initialState);

        machine.Configure(TemplateStatus.Created)
            .Permit(TemplateStatus.ValidatingEligibility, TemplateStatus.ValidatingEligibility)
            .Permit(TemplateStatus.Failed, TemplateStatus.Failed)
            .Permit(TemplateStatus.Cancelled, TemplateStatus.Cancelled);

        return machine;
    }
}
