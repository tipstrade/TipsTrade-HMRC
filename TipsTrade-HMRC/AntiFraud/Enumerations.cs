namespace TipsTrade.HMRC.AntiFraud {
  /// <summary>The valid application connection methods.</summary>
  public enum ConnectionMethod {
    /// <summary>Installed mobile application connecting directly to HMRC.</summary>
    MOBILE_APP_DIRECT,
    /// <summary>Installed desktop application connecting directly to HMRC.</summary>
    DESKTOP_APP_DIRECT,
    /// <summary>Installed mobile application connecting through intermediary servers to HMRC.</summary>
    MOBILE_APP_VIA_SERVER,
    /// <summary>Installed desktop application connecting through intermediary servers to HMRC.</summary>
    DESKTOP_APP_VIA_SERVER,
    /// <summary>Web application connecting through intermediary servers to HMRC.</summary>
    WEB_APP_VIA_SERVER,
    /// <summary>Batch process connecting directly to HMRC.</summary>
    BATCH_PROCESS_DIRECT,
    /// <summary>The application connects directly to HMRC but the method does not fit into the architectures described above.</summary>
    OTHER_DIRECT,
    /// <summary>The application connects through intermediary servers to HMRC but the method does not fit into the architectures described above.</summary>
    OTHER_VIA_SERVER
  }

  /// <summary>The valid multi-factor authentication statuses related to the API call.</summary>
  public enum MFAMethod {
    /// <summary>The MFA was performed by accepting a time-based one-time password code.</summary>
    TOTP,
    /// <summary>The MFA was performed by sending an authorisation code to the user in some out-of-band channel, for example, by email or by SMS</summary>
    AUTH_CODE,
    /// <summary>A different MFA method was used than the available values.</summary>
    OTHER
  }
}
