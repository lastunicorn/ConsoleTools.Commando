using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace DustInTheWind.ConsoleTools.Commando.MetadataModel;

public class ParameterMetadata
{
    private readonly PropertyInfo propertyInfo;
    private readonly CommandParameterAttribute customAttribute;

    private NamedParameterAttribute NamedParameterAttribute => customAttribute as NamedParameterAttribute;

    private AnonymousParameterAttribute AnonymousParameterAttribute => customAttribute as AnonymousParameterAttribute;

    public string Name => NamedParameterAttribute?.Name;

    public char ShortName => NamedParameterAttribute?.ShortName ?? (char)0;

    public string DisplayName
    {
        get
        {
            string displayName = AnonymousParameterAttribute?.DisplayName;

            if (displayName != null)
                return displayName;

            int? order = AnonymousParameterAttribute?.Order;

            if (order != null)
                return $"param {order}";

            return propertyInfo.Name.ToKebabCase();
        }
    }

    public int? Order => AnonymousParameterAttribute?.Order;

    [Obsolete("Replaced by the IsMandatory property.")]
    public bool IsOptional => customAttribute.IsMandatorySelected
        ? !customAttribute.IsMandatory
        : customAttribute.IsOptional;

    public bool IsMandatory => customAttribute.IsMandatorySelected
        ? customAttribute.IsMandatory
        : !customAttribute.IsOptional;

    public string Description => customAttribute.Description;

    public Type ParameterType => propertyInfo.PropertyType;

    public ParameterMetadata(PropertyInfo propertyInfo, CommandParameterAttribute customAttribute)
    {
        this.propertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        this.customAttribute = customAttribute ?? throw new ArgumentNullException(nameof(customAttribute));
    }

    public void SetValue(object consoleCommand, string value)
    {
        bool isFlag = propertyInfo.PropertyType == typeof(bool) && value == null;

        object valueAsObject = isFlag
            ? true
            : ParseValue(value);

        propertyInfo.SetValue(consoleCommand, valueAsObject);
    }

    private object ParseValue(string value)
    {
        bool isListOfNumbers = propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType == typeof(List<int>);
        if (isListOfNumbers)
        {
            return value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .ToList();
        }

        bool isListOfStrings = propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType == typeof(List<string>);
        if (isListOfStrings)
        {
            return value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();
        }

        //bool isList = propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(List<>);
        //if (isList)
        //{
        //    Type itemType = propertyInfo.PropertyType.GetGenericArguments()[0];

        //    object list = value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        //        .Select(x => Convert.ChangeType(x, itemType, formatProvider))
        //        .ToList();

        //    //Type listType = propertyInfo.PropertyType.GetGenericTypeDefinition();
        //    //return Convert.ChangeType(list, listType);

        //    TypeConverter typeConverter2 = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
        //    return typeConverter2.ConvertFrom(list);
        //}

        if (propertyInfo.PropertyType == typeof(Encoding))
        {
            try
            {
                return Encoding.GetEncoding(value);
            }
            catch (Exception ex)
            {
                string parameterDisplayName = Name ?? DisplayName;
                throw new InvalidParameterValueException(parameterDisplayName, value, ex);
            }
        }

        TypeConverter typeConverter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);

        try
        {
            return typeConverter.ConvertFromString(value);
        }
        catch (Exception ex)
        {
            string parameterDisplayName = Name ?? DisplayName;
            throw new InvalidParameterValueException(parameterDisplayName, value, ex);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();

        if (Name != null)
            sb.Append($"Name = {Name}");

        if (Order != null)
        {
            if (sb.Length > 0)
                sb.Append("; ");

            sb.Append($"Order = {Order}");
        }

        if (sb.Length > 0)
            sb.Append("; ");

        sb.Append($"Optional = {IsOptional}");

        return sb.ToString();
    }
}