namespace Parse.Api
{
    internal static class ParseUrls
    {
        // POST to create, GET to query
        public const string Class = "classes/{0}";

        // PUT to update, GET to retreive, DELETE to delete
        public const string ClassObject = "classes/{0}/{1}";

        // POST to sign up, GET to query
        public const string User = "users";

        // PUT to update, GET to retreive, DELETE to delete
        public const string UserObject = "users/{0}";

        // GET to log in
        public const string Login = "login";

        // POST to request password reset
        public const string PasswordReset = "requestPasswordReset";  // TODO

        // POST to create, GET to query
        public const string Role = "roles";// TODO

        // PUT to update, GET to retreive, DELETE to delete
        public const string RoleObject = "roles/{0}";// TODO

        // POST to upload
        public const string File = "files/{FileName}";// TODO

        // POST to track analytics
        public const string AppOpened = "events/AppOpened";

        // POST to send push
        public const string Push = "push";// TODO

        // POST to upload, GET to query
        public const string Installation = "installations";// TODO

        // PUT to update, GET to retreive, DELETE to delete
        public const string InstallationObject = "installations/{0}";// TODO

        // POST to call cloud function
        public const string Function = "functions/{0}";

    }

    internal static class ParseHeaders
    {
        public const string AppId = "X-Parse-Application-Id";
        public const string RestApiKey = "X-Parse-REST-API-Key";
        public const string SessionToken = "X-Parse-Session-Token";
    }
}