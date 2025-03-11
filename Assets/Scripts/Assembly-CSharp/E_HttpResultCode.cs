public enum E_HttpResultCode
{
	None = 0,
	Ok = 200,
	AlreadyProcessed = 208,
	BadRequest = 400,
	Unauthorized = 401,
	Forbidden = 403,
	NotFound = 404,
	RequestTimeout = 408,
	Conflict = 409,
	ExpectationFailed = 417,
	ParameterNotUnderstood = 451,
	InternalServerError = 500,
	ServiceUnavailable = 503
}
