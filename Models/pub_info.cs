﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace personal_project.Models;

public partial class pub_info
{
    public string pub_id { get; set; }

    public byte[] logo { get; set; }

    public string pr_info { get; set; }

    public virtual publisher pub { get; set; }
}