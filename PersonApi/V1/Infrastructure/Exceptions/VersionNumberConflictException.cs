using System;

namespace PersonApi.V1.Infrastructure.Exceptions
{
    public class VersionNumberConflictException : Exception
    {
        public int? IncomingVersionNumber { get; private set; }
        public int? ExpectedVersionNumber { get; private set; }

        public VersionNumberConflictException(int? incoming, int? expected)
            : base(string.Format("The version number supplied ({0}) does not match the current value on the entity ({1}).",
                                 (incoming is null) ? "{null}" : incoming.ToString(),
                                 (expected is null) ? "{null}" : expected.ToString()))
        {
            IncomingVersionNumber = incoming;
            ExpectedVersionNumber = expected;
        }
    }
}
