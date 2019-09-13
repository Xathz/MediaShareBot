using System.Collections.Generic;

namespace MediaShareBot.Extensions {

    public static class GenericsExt {

        public static bool IsDefaultForType<T>(this T type) => EqualityComparer<T>.Default.Equals(type, default);

    }

}
