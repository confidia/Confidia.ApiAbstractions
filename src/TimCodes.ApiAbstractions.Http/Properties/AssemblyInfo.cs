global using TimCodes.ApiAbstractions.Serialization;
global using TimCodes.ApiAbstractions.Models.Requests;
global using TimCodes.ApiAbstractions.Models.Responses;
global using TimCodes.ApiAbstractions.Http.Requests;
global using TimCodes.ApiAbstractions.Http.Responses;
global using TimCodes.ApiAbstractions.Authorization;
global using TimCodes.ApiAbstractions.Http.Constants;
global using TimCodes.ApiAbstractions.Http.Serialization;
global using TimCodes.ApiAbstractions.Mapping;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.DependencyInjection;
global using System.Net.Http.Headers;
global using System.Text.Json;
global using System.Net;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TimCodes.ApiAbstractions.Http.Server")]