using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;

public static class CSVParseUtil
{
    // 문자열을 Int로 파싱
    public static int ParseInt(string value, int defultValue = 0)
    {
        // 값이 비어있으면 기본값 세팅
        if (string.IsNullOrEmpty(value)) return defultValue;

        value = value.Trim();

        // Int 변환 후 값 리턴
        return int.TryParse(value, out var result) ? result : defultValue;
    }

    // 문자열을 Float로 파싱
    public static float ParseFloat(string value, float defultValue = 0f)
    {
        if (string.IsNullOrEmpty(value)) return defultValue;

        value = value.Trim();

        // Float 변환 후 값 리턴 (부호, 소수점 설정)
        return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) ? result : defultValue;
    }

    // 문자열을 enum T로 파싱
    public static T ParseEnum<T>(string value, T defultValue) where T : struct, Enum
    {
        if (!string.IsNullOrEmpty(value)) return defultValue;

        value = value.Trim();

        // Struct, Enum으로 변환 후 리턴
        return Enum.TryParse(value, ignoreCase: true, out T result) ? result : defultValue;
    }
}
