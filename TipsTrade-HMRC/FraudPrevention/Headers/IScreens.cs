using System.Collections.Generic;
using System.Linq;

namespace TipsTrade.HMRC.FraudPrevention.Headers {
  /// <summary>Provides the Gov-Client-Screens header.</summary>
  public interface IScreens {
    /// <summary>Gets or sets the list of information related to the originating device's screens.</summary>
    IEnumerable<Screen>? Screens { get; set; }
  }

  /// <summary>
  /// Extension methods for <see cref="IScreens"/> to populate the screens and generate the corresponding header.
  /// </summary>
  public static class ScreensExtensions {
    internal static FraudPreventionHeader GetScreensHeader(this IScreens source) =>
      new FraudPreventionHeader("Gov-Client-Screens", false, source.Screens);

#if NETFRAMEWORK
    /// <summary>Populates <see cref="IScreens.Screens"/> with all screens in the system.</summary>
    public static void PopulateScreens(this IScreens source) {
      source.Screens = System.Windows.Forms.Screen.AllScreens.Select(x => (Screen)x);
    }
#endif 
  }
}
