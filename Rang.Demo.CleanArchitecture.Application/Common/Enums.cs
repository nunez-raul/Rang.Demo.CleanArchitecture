﻿namespace Rang.Demo.CleanArchitecture.Application.Common
{
    public enum CommandResultStatusCode
    {
        Success,
        InvalidInput,
        FailedModelValidation,
        DuplicateEntry,
        MissingClub,
        MissingUsersToAdd,
        ClubNotFound,
        UserNotFound,
        UsersInListNotFound
    }
}
