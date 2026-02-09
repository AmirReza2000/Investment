//using System;
//using System.Collections.Generic;
//using System.Text;
//using Investment.Domain;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

//namespace Investment.Infrastructure.Domain.Users;

//internal static class EmailAddressConverter
//{
//    public static readonly ValueConverter<EmailAddress, string> EmailConverter =
//        new(
//            vo => vo.Value,                
//            value => EmailAddress.Create(value) 
//        );
//}
