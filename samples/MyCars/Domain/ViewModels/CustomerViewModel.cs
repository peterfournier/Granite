﻿using GraniteCore;
using GraniteCore.MVC.ViewModels;
using System;

namespace MyCars.Domain.ViewModels
{
    public class CustomerViewModel : UserBasedModel<Guid, UserViewModel, string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime InceptionDate { get; set; }

        //public new UserViewModel CreatedByUser { get; set; }
        //public new UserViewModel LastModifiedByUser { get; set; }
    }
}
