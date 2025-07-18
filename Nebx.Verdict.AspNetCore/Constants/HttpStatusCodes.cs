namespace Nebx.Verdict.AspNetCore.Constants;

internal enum HttpStatusCodes
{
    InternalServerError = 500,
    BadRequest = 400,
    NotFound = 404,
    Unauthorized = 401,
    Forbidden = 403,
    UnprocessableEntity = 422,
    Conflict = 409,
    Success = 200,
    Created = 201,
    NoContent = 204,
}