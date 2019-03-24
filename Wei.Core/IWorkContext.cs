using Wei.Core.Domain.Localization;
using Wei.Core.Domain.Users;
using System.Collections;
using System.Collections.Generic;

namespace Wei.Core
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        User CurrentUser { get; set; }
        ///// <summary>
        ///// Gets or sets the original customer (in case the current one is impersonated)
        ///// </summary>
        //User OriginalUserIfImpersonated { get; }
        ///// <summary>
        ///// Gets or sets the current vendor (logged-in manager)
        ///// </summary>
        //Vendor CurrentVendor { get; }

        /// <summary>
        /// Get or set current user working language
        /// </summary>
        Language WorkingLanguage { get; set; }
        ///// <summary>
        ///// Get or set current user working currency
        ///// </summary>
        //Currency WorkingCurrency { get; set; }
        ///// <summary>
        ///// Get or set current tax display type
        ///// </summary>
        //TaxDisplayType TaxDisplayType { get; set; }

        /// <summary>
        /// Get or set value indicating whether we're in admin area
        /// </summary>
        bool IsAdmin { get; set; }

        /// <summary>
        /// 获取表中列的描述
        /// tname / description / colname
        /// </summary>
        IDictionary<string, IDictionary<string, string>> ColDescription { get; }

    }
}
