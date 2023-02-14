﻿using MoviesAPI.DTOs;

namespace MoviesAPI.Helper
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
        {
            return queryable
                .Skip((paginationDTO.Page - 1) * paginationDTO.RecordPerPage)
                .Take(paginationDTO.RecordPerPage);
        }
    }
}
