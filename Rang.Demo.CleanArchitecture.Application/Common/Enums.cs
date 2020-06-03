namespace Rang.Demo.CleanArchitecture.Application.Common
{
    public enum CommandResultStatusCode
    {
        Success,
        InvalidInput,
        FailedModelValidation,
        DuplicateEntry,
        OverlappingRanges,
        UserNotFound,
        FeedbackReceiverNotFound,
        FeedbackReceiverMissing,
        FeedbackGiverNotFound,
        FeedbackGiverMissing,
        FeedbackPeriodNotFound,
        AppraisalNotFound,
        FeedbackNotFound,
        EvaluationDone,
        EntityNotUpdatable,
        EntityNotDeletable,
        InvitationExpired,
        InvitationNotFound,
        SomeFeedbackGiversInListNotFound,
        SomeFeedbackReceiversInListNotFound,
        SomeFeedbackPeriodsInListNotFound
    }
}
