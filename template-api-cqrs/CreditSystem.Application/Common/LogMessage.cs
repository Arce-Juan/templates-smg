namespace Template.Application.Common;

public static class LogMessage
{
    public static class ProcessHandler
    {
        public const string TemplateNotFound = "Template not found for RequestId {RequestId}";
        public const string InvalidTransition = "Invalid transition from {Status} for RequestId {RequestId}";
        public const string RequestNotInEvaluatingRisk = "Request not in EvaluatingRisk (current: {Status}) for RequestId {RequestId}";
        public const string StepFailedReasonTemplateNotFound = "Template not found";
        public const string StepFailedReasonInvalidTransitionFormat = "Invalid transition from current state {0}";
        public const string EligibilityStepFailed = "Eligibility step failed for RequestId {RequestId}";
        public const string RiskStepFailed = "Risk step failed for RequestId {RequestId}";
        public const string ConditionsStepFailed = "Conditions step failed for RequestId {RequestId}";
        public const string DecisionStepFailed = "Decision step failed for RequestId {RequestId}";
        public const string StartingEligibility = "Starting Eligibility step for RequestId {RequestId}";
        public const string StartingRisk = "Starting Risk step for RequestId {RequestId}";
        public const string StartingConditions = "Starting Conditions step for RequestId {RequestId}";
        public const string StartingDecision = "Starting Decision step for RequestId {RequestId}";
        public const string EligibilityValidated = "Eligibility validated for RequestId {RequestId}";
        public const string EligibilityRejected = "Eligibility rejected for RequestId {RequestId}: {Reason}";
        public const string RiskEvaluated = "Risk evaluated for RequestId {RequestId}";
        public const string RiskRejected = "Risk rejected for RequestId {RequestId}: {Reason}";
        public const string ConditionsCalculated = "Conditions calculated for RequestId {RequestId}";
        public const string DecisionIssued = "Decision issued (Approved) for RequestId {RequestId}";
    }
}
