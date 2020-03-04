using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixIt_Backend.Extensions
{
    /// <summary>
    /// HTTP Filters parsed in a pretty manner.
    /// </summary>
    public class HttpFilter
    {
        /// <summary>
        /// Gets or Sets the query filter.
        /// </summary>
        public string Q { get; set; }

        /// <summary>
        /// Gets or Sets the begin index.
        /// </summary>
        public int BeginIndex { get; set; } = 0;

        /// <summary>
        /// Gets or Sets the limit page size.
        /// </summary>
        public int Limit { get; set; } = 25;

        /// <summary>
        /// Gets or sets the primary sort column.
        /// </summary>
        public string SortBy { get; set; } = "id";

        /// <summary>
        /// Gets the calculated end index.
        /// </summary>
        public int EndIndex => BeginIndex + Limit + 1;

        /// <summary>
        /// Gets or sets the list of ids for filtering.
        /// </summary>
        public List<int> Id { get; set; }

        /// <summary>
        /// This id is the foreign key of a model. 
        /// </summary>
        public int ReferenceId { get; set; }

        /// <summary>
        /// The order of sorting either ascending or descending.
        /// </summary>
        public string SortOrder { get; set; } = "ASC";

        /// <summary>
        /// Gets or sets the custom filters besides Q and Id.
        ///
        /// Suppose user wants to filter feedback using message and user Id, they would call
        /// `/feedback?filter={q: 'some_message', userIds: [1,2,3]`.
        /// The CustomFilters will store filters besides Q and Id (in this case, userIds).
        /// </summary>
        public Dictionary<string, object> CustomFilters = new Dictionary<string, object>();
    }

    /// <summary>
    /// Filter extensions for controller.
    /// </summary>
    public static class FilterExtension
    {
        /// <summary>
        /// Get HTTP Filter Parameters.
        /// </summary>
        /// <param name="controller">The Authorized Web API Controller.</param>
        /// <returns>HTTP Filter parameters..</returns>
        public static HttpFilter GetFilters(this ControllerBase controller)
        {
            var filter = controller.HttpContext.Request.Query["filter"];
            var range = controller.HttpContext.Request.Query["range"];
            var sort = controller.HttpContext.Request.Query["sort"];
            var filterObj = !string.IsNullOrEmpty(filter)
                ? JsonConvert.DeserializeObject<Dictionary<string, object>>(filter)
                : null;
            var rangeObj = !string.IsNullOrEmpty(range) ? JsonConvert.DeserializeObject<List<int>>(range) : null;
            var sortObj = !string.IsNullOrEmpty(sort) ? JsonConvert.DeserializeObject<List<string>>(sort) : null;

            var httpFilters = new HttpFilter();

            if (filterObj != null)
            {
                if (filterObj.ContainsKey("q"))
                {
                    httpFilters.Q = (string)filterObj["q"];
                    filterObj.Remove("q");
                }

                if (filterObj.ContainsKey("id"))
                {
                    var listOfIds = JsonConvert.DeserializeObject<List<int>>(filterObj["id"].ToString());
                    httpFilters.Id = listOfIds;
                    filterObj.Remove("id");
                }

                httpFilters.CustomFilters = filterObj;

                if (httpFilters.CustomFilters.Count() > 0)
                {
                    httpFilters.ReferenceId = JsonConvert.DeserializeObject<int>(httpFilters.CustomFilters.ElementAt(0).Value.ToString());
                }
            }

            if (rangeObj != null)
            {
                httpFilters.BeginIndex = rangeObj[0];
                httpFilters.Limit = rangeObj[1] - rangeObj[0] + 1;
            }
            else
            {
                httpFilters.BeginIndex = 0;
                httpFilters.Limit = 25;
            }

            if (sortObj != null)
            {
                httpFilters.SortBy = sortObj[0];
                httpFilters.SortOrder = sortObj[1];
            }

            return httpFilters;
        }

    }

}
