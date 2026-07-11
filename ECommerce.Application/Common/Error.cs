using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Common
{
    public record Error (string code , string description , ErrorType Type=ErrorType.Failure)
    {
        public static Error Failure( string Code = "General.Failure" , string descriopion = "General Failure Desc ")
            => new Error(Code, descriopion , ErrorType.Failure);
        public static Error validation(string Code = "General.validation", string descriopion = "General validation Desc ")
            => new Error(Code, descriopion, ErrorType.validation);
        public static Error NotFound(string Code = "General.NotFound", string descriopion = "General NotFound Desc ")
            => new Error(Code, descriopion, ErrorType.NotFound);
        public static Error Conflict(string Code = "General.Conflict", string descriopion = "General Conflict Desc ")
            => new Error(Code, descriopion, ErrorType.Conflict);
        public static Error UnAuthorized(string Code = "General.UnAuthorized", string descriopion = "General UnAuthorized Desc ")
            => new Error(Code, descriopion, ErrorType.UnAuthorized);
        public static Error Fotbidden(string Code = "General.Fotbidden", string descriopion = "General Fotbidden Desc ")
            => new Error(Code, descriopion, ErrorType.Fotbidden);
        public static Error Invaildcredentails(string Code = "General.Invaildcredentails", string descriopion = "General Invaildcredentails Desc ")
            => new Error(Code, descriopion, ErrorType.Invaildcredentails);

    }
}
