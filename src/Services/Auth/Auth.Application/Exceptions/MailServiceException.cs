﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Exceptions;

public class MailServiceException(string msg) : Exception(msg)
{
}
