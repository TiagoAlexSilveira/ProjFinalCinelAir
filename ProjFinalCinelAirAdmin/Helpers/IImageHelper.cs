﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Helpers
{
    public interface IImageHelper
    {
        Task<string> UpLoadImageAsync(IFormFile imageFile, string folder);
    }
}
