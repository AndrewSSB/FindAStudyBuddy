﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectMDS.DAL.Models.UserModels
{
    public class ForgotPassModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Current password is required")]
        public string currentPassword { get; set; }
        [Required(ErrorMessage = "New password is required")]
        public string newPassword { get; set; }
        [Required(ErrorMessage = "Confirm new password is required")]
        public string confirmNewPassord { get; set; }
    }
}
