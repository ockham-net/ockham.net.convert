# To generate aliased for Convert, use $instance = $false
# To generate aliased methods for Converter, use $instance = $true
$instance = $true
$reference = $false
$includeForce = $false

$typeNames = @(
   $null ,
 # @( 'Int16'   , 'Short'    , 'Short'    , 'short'   , '16-bit signed integer', 'integer', '0' ),
   @( 'Int32'   , 'Int'      , 'Integer'  , 'int'     , '32-bit signed integer', 'integer', '0' ),
   @( 'Int64'   , 'Lng'      , 'Long'     , 'long'    , '64-bit signed integer', 'integer', '0' ),
 # @( 'UInt16'  , 'UShort'   , 'UShort'   , 'ushort'  , '16-bit unsigned integer', 'integer', '0' ),
 # @( 'UInt32'  , 'UInt'     , 'UInteger' , 'uint'    , '32-bit unsigned integer', 'integer', '0' ),
 # @( 'UInt64'  , 'ULng'     , 'ULong'    , 'ulong'   , '64-bit unsigned integer', 'integer', '0' ),
 # @( 'Single'  , 'Sng'      , 'Single'   , 'float'   , 'single-precision floating point number', 'floating point', '0' ),
   @( 'Boolean' , 'Bool'     , 'Boolean'  , 'bool'    , 'boolean', 'boolean', 'False' ),
   @( 'Double'  , 'Dbl'      , 'Double'   , 'double'  , 'double-precision floating point number', 'floating point', '0' ),
   @( 'Decimal' , 'Dec'      , 'Decimal'  , 'decimal' , 'exact decimal', 'decimal', '0' ),
 # @( 'Byte'    , 'Byte'     , 'Byte'     , 'byte'    , '8-bit unsigned integer', 'integer', '0' ),
 # @( 'SByte'   , 'SByte'    , 'SByte'    , 'sbyte'   , '8-bit signed integer', 'integer', '0' ),
   @( 'String'  , 'Str'      , 'String'   , 'string'  , 'string', 'string', 'null' ),
   @( 'DateTime', 'Date'     , 'Date'     , 'DateTime', 'date time', 'date or time', 'DateTime.MinValue' ),
   @( 'TimeSpan', 'TimeSpan' , 'TimeSpan' , 'TimeSpan', 'timespan', 'timespan', 'Timespan.zero' ),
   @( 'Guid    ', 'Guid'     , 'Guid'     , 'Guid'    , '128-bit globally unique identifier', 'GUID', 'Guid.Empty' )
) | % { 

    if($_ -eq $null) { return }

    New-Object psobject -Property @{
        Name = $_[0]
        Alias = $_[1]
        VBName = $_[2]
        CSharpName = $_[3]
        Description = $_[4]
        ShortName = $_[5]
        Default = $_[6]
    } 
} | Sort-Object Name

$typeNames | Format-Table


$converts  = New-Object 'System.Collections.Generic.List[String]'
$force     = New-Object 'System.Collections.Generic.List[String]'
$forceDflt = New-Object 'System.Collections.Generic.List[String]'
$classBaseName = $(if($instance) { 'Ockham.Data.Converter' } else { 'Ockham.Data.Convert' })
$modifier   = $(if($instance) { '' } else { 'static ' })

$typeNames | ForEach-Object { 

$name = $_.Name
$csName = $_.CSharpName 
$desc = $_.Description
$shortName = $_.ShortName
$dflt = $_.Default
$csCamelName = $csName[0].ToString().ToLower() + $csName.Substring(1)



if($reference) {
    $converts.Add(@"
    public $($modifier)$csName To$($name)(object value) => throw null;    
"@)

    if($includeForce -and !$instance) {
        $force.Add(@"
    public static $csName Force$($name)(object value) => throw null;        
"@)

        $forceDflt.Add(@" 
    public static $csName Force$($name)(object value, $csName defaultValue) => throw null;        
"@)
    }
} else {

    $options = $(if($instance) { 'this.Options' } else { 'ConvertOptions.Default' });
    $call    = $(if($csName -eq 'string') {
        "($csName)Convert.ToReference(value, typeof($csName), $options, false, null)"
    } else {
        "Convert.ToStruct<$csName>(value, $options)"
    })
     
    $converts.Add(@"
        /// <summary>
        /// Convert any input value to an equivalent $desc value. Attempting to convert
        /// a value that has no meaningful $shortName equivalent, including an empty value,
        /// will cause an exception to be raised.
        /// </summary>
        /// <param name="value">Any value</param>
        public $($modifier)$csName To$($name)(object value) {
            if(value is $csName $($csCamelName)Value) return $($csCamelName)Value;
            return $call;
        }

"@)  
     

    if($includeForce -and !$instance) {
        $force.Add(@"
    /// <summary>
    /// Convert any input value to an equivalent $desc value. If the input is empty
    /// or has no meaningful $shortName equivalent, $dflt is returned.
    /// </summary>
    /// <param name="value">Any value</param>
    /// <seealso cref="Ockham.Data.Convert.Force{T}(Object)"/>
    public static $csName Force$($name)(object value) {
        if(value is $csName $($csCamelName)Value) return $($csCamelName)Value;
        return Force<$csName>(value);
    } 
    
"@)

        $forceDflt.Add(@"
    /// <summary>
    /// Convert any input value to an equivalent $desc value. If the input is empty
    /// or has no meaningful $shortName equivalent the provided default value is returned. 
    /// </summary>
    /// <param name="value">Any value</param>
    /// <param name="defaultValue">A value to return if the input is empty or cannot be converted</param>
    /// <seealso cref="Ockham.Data.Convert.Force{T}(Object, T)"/>
    public static $csName Force$($name)(object value, $csName defaultValue) {
        if(value is $csName $($csCamelName)Value) return $($csCamelName)Value;
        return Force<$csName>(value, defaultValue);
    }  
    
"@)
    } # if(!$instance)
} # if($reference)
} # %

[string[]] $vals = @() + $converts + $force + $forceDflt

# $vals | Set-Clipboard
$vals