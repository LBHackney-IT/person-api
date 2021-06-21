// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~P:PersonApi.V1.Domain.Tenure.IsActive")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:PersonApi.V1.Gateways.PersonSnsGateway.Publish(PersonApi.V1.Domain.PersonSns)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Usage", "CA2200:Rethrow to preserve stack details.", Justification = "<Pending>", Scope = "member", Target = "~M:PersonApi.V1.UseCase.PostNewPersonUseCase.ExecuteAsync(PersonApi.V1.Boundary.Request.PersonRequestObject,PersonApi.V1.Infrastructure.JWT.Token)~System.Threading.Tasks.Task{PersonApi.V1.Boundary.Response.PersonResponseObject}")]
[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>", Scope = "member", Target = "~F:PersonApi.V1.Infrastructure.Constants.V1_VERSION")]
[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>", Scope = "member", Target = "~F:PersonApi.V1.Infrastructure.Constants.SOURCE_DOMAIN")]
[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>", Scope = "member", Target = "~F:PersonApi.V1.Infrastructure.Constants.SOURCE_SYSTEM")]
[assembly: SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:PersonApi.V1.Infrastructure.JWT.Token.Groups")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "type", Target = "~T:PersonApi.V1.Domain.Tenure")]
