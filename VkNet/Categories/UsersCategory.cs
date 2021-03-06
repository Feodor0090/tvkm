using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using JetBrains.Annotations;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkNet.Categories;

/// <inheritdoc />
public partial class UsersCategory : IUsersCategory
{
    private readonly IVkApiInvoke _vk;

    /// <summary>
    ///     Api vk.com
    /// </summary>
    /// <param name="vk"> </param>
    public UsersCategory(IVkApiInvoke vk)
    {
        _vk = vk;
    }

    /// <inheritdoc />
    [Pure]
    public VkCollection<User> Search(UserSearchParams @params)
    {
        return _vk.Call("users.search", new VkParameters
        {
            {"q", WebUtility.HtmlEncode(@params.Query)}, {"sort", @params.Sort}, {"offset", @params.Offset},
            {"count", @params.Count}, {"fields", @params.Fields}, {"city", @params.City}, {"country", @params.Country},
            {"hometown", WebUtility.HtmlEncode(@params.Hometown)}, {"university_country", @params.UniversityCountry},
            {"university", @params.University}, {"university_year", @params.UniversityYear},
            {"university_faculty", @params.UniversityFaculty}, {"university_chair", @params.UniversityChair},
            {"sex", @params.Sex}, {"status", @params.Status}, {"age_from", @params.AgeFrom}, {"age_to", @params.AgeTo},
            {"birth_day", @params.BirthDay}, {"birth_month", @params.BirthMonth}, {"birth_year", @params.BirthYear},
            {"online", @params.Online}, {"has_photo", @params.HasPhoto}, {"school_country", @params.SchoolCountry},
            {"school_city", @params.SchoolCity}, {"school_class", @params.SchoolClass}, {"school", @params.School},
            {"school_year", @params.SchoolYear}, {"religion", WebUtility.HtmlEncode(@params.Religion)},
            {"interests", WebUtility.HtmlEncode(@params.Interests)},
            {"company", WebUtility.HtmlEncode(@params.Company)}, {"position", WebUtility.HtmlEncode(@params.Position)},
            {"group_id", @params.GroupId}, {"from_list", @params.FromList}
        }).ToVkCollectionOf<User>(r => r);
    }

    /// <inheritdoc />
    [Pure]
    public bool IsAppUser(long? userId)
    {
        var parameters = new VkParameters
        {
            {"user_id", userId}
        };

        return _vk.Call("users.isAppUser", parameters);
    }

    /// <inheritdoc />
    [Pure]
    public ReadOnlyCollection<User> Get(IEnumerable<long> userIds
        , ProfileFields fields = null
        , NameCase nameCase = null)
    {
        if (userIds == null) throw new ArgumentNullException(nameof(userIds));

        var parameters = new VkParameters
        {
            {"fields", fields}, {"name_case", nameCase}, {"user_ids", userIds}
        };

        VkResponseArray response = _vk.Call("users.get", parameters);

        return response.ToReadOnlyCollectionOf<User>(x => x);
    }

    /// <inheritdoc />
    [Pure]
    [NotNull]
    [ContractAnnotation("screenNames:null => halt")]
    public ReadOnlyCollection<User> Get(IEnumerable<string> screenNames
        , ProfileFields fields = null
        , NameCase nameCase = null)
    {
        if (screenNames == null) throw new ArgumentNullException(nameof(screenNames));

        var parameters = new VkParameters
        {
            {"user_ids", screenNames}, {"fields", fields}, {"name_case", nameCase}
        };

        VkResponseArray response = _vk.Call("users.get", parameters);

        return response.ToReadOnlyCollectionOf<User>(x => x);
    }

    /// <inheritdoc />
    [Pure]
    public VkCollection<Group> GetSubscriptions(long? userId = null
        , int? count = null
        , int? offset = null
        , GroupsFields fields = null)
    {
        VkErrors.ThrowIfNumberIsNegative(() => userId);
        VkErrors.ThrowIfNumberIsNegative(() => count);
        VkErrors.ThrowIfNumberIsNegative(() => offset);

        var parameters = new VkParameters
        {
            {"user_id", userId}, {"extended", true}, {"offset", offset}, {"count", count}, {"fields", fields}
        };

        return _vk.Call("users.getSubscriptions", parameters)
            .ToVkCollectionOf<Group>(x => x);
    }

    /// <inheritdoc />
    [Pure]
    public VkCollection<User> GetFollowers(long? userId = null
        , int? count = null
        , int? offset = null
        , ProfileFields fields = null
        , NameCase nameCase = null)
    {
        VkErrors.ThrowIfNumberIsNegative(() => userId);
        VkErrors.ThrowIfNumberIsNegative(() => count);
        VkErrors.ThrowIfNumberIsNegative(() => offset);

        var parameters = new VkParameters
        {
            {"user_id", userId}, {"offset", offset}, {"count", count}, {"fields", fields}, {"name_case", nameCase}
        };

        return _vk.Call("users.getFollowers", parameters)
            .ToVkCollectionOf(x => x.ContainsKey("id") ? x : new User {Id = x});
    }

    /// <inheritdoc />
    public bool Report(long userId, ReportType type, string comment = "")
    {
        VkErrors.ThrowIfNumberIsNegative(() => userId);

        var parameters = new VkParameters
        {
            {"user_id", userId}, {"type", type}, {"comment", comment}
        };

        return _vk.Call("users.report", parameters);
    }

    /// <inheritdoc />
    public VkCollection<User> GetNearby(UsersGetNearbyParams @params)
    {
        return _vk.Call("users.getNearby", new VkParameters
        {
            {
                "latitude", @params.Latitude.ToString(CultureInfo.InvariantCulture)
            }, //Vk API не принимает дробные числа с запятой, нужна точка
            {"longitude", @params.Longitude.ToString(CultureInfo.InvariantCulture)}, {"accuracy", @params.Accuracy},
            {"timeout", @params.Timeout}, {"radius", @params.Radius}, {"fields", @params.Fields},
            {"name_case", @params.NameCase}, {"need_description", @params.NeedDescription}
        }).ToVkCollectionOf<User>(x => x);
    }

    /// <inheritdoc cref="User" />
    [Pure]
    public User Get(long userId, ProfileFields fields = null, NameCase nameCase = null)
    {
        VkErrors.ThrowIfNumberIsNegative(() => userId);
        var users = Get(new[] {userId}, fields, nameCase);

        return users.FirstOrDefault();
    }

    /// <inheritdoc cref="User" />
    public User Get([NotNull] string screenName
        , ProfileFields fields = null
        , NameCase nameCase = null)
    {
        VkErrors.ThrowIfNullOrEmpty(() => screenName);

        var users = Get(new[] {screenName}, fields, nameCase);

        return users.Count > 0 ? users[0] : null;
    }
}