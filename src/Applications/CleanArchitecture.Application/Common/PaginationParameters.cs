﻿namespace CleanArchitecture.Application.Common;

public class PaginationParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public int OffSet => (PageNumber - 1) * PageSize;
}