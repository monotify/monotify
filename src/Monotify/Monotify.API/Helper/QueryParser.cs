using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;

namespace Monotify.API.Helper
{
    /// <summary>
    /// This QueryParser will help you to parse from Query and Sorting from Url
    /// Example: /log?filter[created>2017-01-01]
    /// Example: /log?sort[+created]
    /// Example: /log?page=2
    /// </summary>
    public class QueryParser
    {
        #region Consts

        private const string AscOperator = "+";
        private const string DescOperator = "-";

        private const string QueryRegex = @"\?filter\[(?<filter>.[^\]]+)\]|sort\[(?<sort>.[^\]]+)\]|page=(?<page>[\d]+)|per_page=(?<per_page>[\d]+)";
        private const string FilterGroupName = "filter";
        private const string SortGroupName = "sort";
        private const string PageGroupName = "page";
        private const string PerPageGroupName = "per_page";
        private const string FilterParseRegex = @"(?<field>[^?=&<>!\*\^\$]+)((?<opr>[\*\^\$]=|!=|<=|>=|>|<|=)(?<value>[^&]*))?";
        private const string FilterFieldName = "field";
        private const string FilterValueName = "value";
        private const string FilterOperatorName = "opr";
        private const string SortParseRegex = @"(?<direction>\+|-)?(?<field>[\w.]+)";
        private const string SortFieldName = @"field";
        private const string SortDirectionName = @"direction";
        private const string NullComparison = "(null)";
        private const string OprEqual = "=";
        private const string OprNotEqual = "!=";
        private const string OprGreater = ">";
        private const string OprGreaterThanOrEqual = ">=";
        private const string OprLessThan = "<";
        private const string OprLessThanOrEqual = "<=";
        private const string OprStartsWith = "^=";
        private const string OprEndsWith = "$=";
        private const string OprContains = "*=";

        #endregion

        public List<QueryFilter> Filters { get; set; } = new List<QueryFilter>();
        public List<QuerySort> Sorts { get; set; } = new List<QuerySort>();
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 20;

        public QueryParser(string query)
        {
            query = WebUtility.UrlDecode(query);

            var rex = new Regex(QueryRegex);
            var matches = rex.Matches(query);
            if (matches.Count == 0) return;

            foreach (Match match in matches)
            {
                if (!string.IsNullOrEmpty(match.Groups[FilterGroupName]?.Value))
                    ParseFilter(match.Groups[FilterGroupName].Value);
                if (!string.IsNullOrEmpty(match.Groups[SortGroupName]?.Value))
                    ParseSort(match.Groups[SortGroupName].Value);
                if (!string.IsNullOrEmpty(match.Groups[PageGroupName]?.Value))
                    ParsePage(match.Groups[PageGroupName].Value);
                if (!string.IsNullOrEmpty(match.Groups[PerPageGroupName]?.Value))
                    ParsePerPage(match.Groups[PerPageGroupName].Value);
            }
        }

        private void ParseFilter(string query)
        {
            query = Uri.UnescapeDataString(Uri.UnescapeDataString(query));

            var rexFilter = new Regex(FilterParseRegex);

            var matches = rexFilter.Matches(query);
            foreach (Match match in matches)
            {
                string field = null, opr = null;
                object value = null;

                if (!string.IsNullOrEmpty(match.Groups[FilterFieldName].Value))
                    field = match.Groups[FilterFieldName].Value;
                if (!string.IsNullOrEmpty(match.Groups[FilterValueName].Value))
                    value = match.Groups[FilterValueName].Value;
                if (!string.IsNullOrEmpty(match.Groups[FilterOperatorName].Value))
                    opr = match.Groups[FilterOperatorName].Value;

                if (NullComparison.Equals(value))
                    value = null;

                var filter = new QueryFilter
                {
                    Field = field,
                    Value = value,
                    Operator = QueryOperator.Equal
                };

                switch (opr)
                {
                    case OprEqual:
                        filter.Operator = value == null ? QueryOperator.IsNull : QueryOperator.Equal;
                        break;
                    case OprNotEqual:
                        filter.Operator = value == null ? QueryOperator.IsNotNull : QueryOperator.NotEqual;
                        break;
                    case OprGreater:
                        filter.Operator = QueryOperator.GreaterThan;
                        break;
                    case OprGreaterThanOrEqual:
                        filter.Operator = QueryOperator.GreaterThanOrEqual;
                        break;
                    case OprLessThan:
                        filter.Operator = QueryOperator.LessThan;
                        break;
                    case OprLessThanOrEqual:
                        filter.Operator = QueryOperator.LessThanOrEqual;
                        break;
                    case OprContains:
                        filter.Operator = QueryOperator.Contains;
                        break;
                    case OprStartsWith:
                        filter.Operator = QueryOperator.StartsWith;
                        break;
                    case OprEndsWith:
                        filter.Operator = QueryOperator.EndsWith;
                        break;
                    default:
                        break;
                }

                Filters.Add(filter);
            }
        }

        private void ParseSort(string query)
        {
            var rexFilter = new Regex(SortParseRegex);

            var matches = rexFilter.Matches(query);
            foreach (Match match in matches)
            {
                string field = null;
                var direction = true;

                if (!string.IsNullOrEmpty(match.Groups[SortFieldName].Value))
                    field = match.Groups[SortFieldName].Value;
                if (!string.IsNullOrEmpty(match.Groups[SortDirectionName].Value))
                    direction = match.Groups[SortDirectionName].Value == AscOperator;

                var filter = new QuerySort
                {
                    Field = field,
                    Asc = direction
                };

                Sorts.Add(filter);
            }
        }

        private void ParsePage(string query)
        {
            if (!string.IsNullOrEmpty(query))
                Page = Convert.ToInt32(query);
        }

        private void ParsePerPage(string query)
        {
            if (!string.IsNullOrEmpty(query))
                PerPage = Convert.ToInt32(query);
        }
    }

    public class QueryFilter
    {
        public string Field { get; set; }
        public object Value { get; set; }
        public QueryOperator Operator { get; set; }
    }

    public class QuerySort
    {
        public string Field { get; set; }
        public bool Asc { get; set; } = true;
    }

    public enum QueryOperator
    {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        IsNull,
        IsNotNull,
        StartsWith,
        EndsWith,
        Contains
    }
}