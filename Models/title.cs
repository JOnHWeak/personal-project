﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace personal_project.Models;

public partial class title
{
    public string title_id { get; set; }

    public string title1 { get; set; }

    public string type { get; set; }

    public string pub_id { get; set; }

    public decimal? price { get; set; }

    public decimal? advance { get; set; }

    public int? royalty { get; set; }

    public int? ytd_sales { get; set; }

    public string notes { get; set; }

    public DateTime pubdate { get; set; }

    public virtual publisher pub { get; set; }

    public virtual ICollection<sale> sales { get; set; } = new List<sale>();

    public virtual ICollection<titleauthor> titleauthors { get; set; } = new List<titleauthor>();
}