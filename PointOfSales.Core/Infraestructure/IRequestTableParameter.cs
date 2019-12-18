﻿namespace PointOfSales.Core.Infraestructure
{
    public interface IRequestTableParameter
    {
        PropertyOrder[] OrderBy { get; set; }
        int Page { get; set; }
        string Query { get; set; }
        bool IsFilterByColumn { get; set; }
    }
}