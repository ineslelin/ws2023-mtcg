﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Server.Req
{
    internal interface IRequestHandler
    {
        void HandleUserRequest();
    }
}