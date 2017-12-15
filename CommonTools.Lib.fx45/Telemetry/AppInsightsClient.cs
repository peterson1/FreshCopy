using CommonTools.Lib.fx45.FileSystemTools;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;

namespace CommonTools.Lib.fx45.Telemetry
{
    public class AppInsightsClient
    {
        private readonly TelemetryClient _telemetryClient;
        private string _sessionKey;
        private string _userName;
        private string _osName;
        private string _version;
        private string _application;
        private string _manufacturer;
        private string _model;


        public AppInsightsClient(string instrumentationKey)
        {
            _telemetryClient = new TelemetryClient()
                { InstrumentationKey = instrumentationKey };
            GatherDetails();
            SetupTelemetry();
        }


        public void PostEvent(string eventName)
        {
            _telemetryClient.TrackEvent(eventName);
            FlushData();
        }


        public void TrackPageView(string pageName)
        {
            _telemetryClient.TrackPageView(pageName);
            FlushData();
        }


        public void TrackNonFatalExceptions(Exception ex, string errorContext)
        {
            var metrics = new Dictionary<string, double>
                { { errorContext, 1 } };
            _telemetryClient.TrackException(ex, null, metrics);
            FlushData();
        }


        public void TrackFatalException(Exception ex, string errorContext)
        {
            var exceptionTelemetry = new ExceptionTelemetry(new Exception(errorContext));
            //todo: replace obsolete call
#pragma warning disable CS0618 // Type or member is obsolete
            exceptionTelemetry.HandledAt = ExceptionHandledAt.Unhandled;
#pragma warning restore CS0618 // Type or member is obsolete
            _telemetryClient.TrackException(exceptionTelemetry);
            FlushData();
        }


        private void FlushData()
        {
#if !DEBUG
            _telemetryClient.Flush();
#endif
        }


        private void SetupTelemetry()
        {
            _telemetryClient.Context.Properties.Add("Application Version", _version);
            _telemetryClient.Context.User.Id = _userName;
            _telemetryClient.Context.User.UserAgent = _application;
            _telemetryClient.Context.Component.Version = _version;

            _telemetryClient.Context.Session.Id = _sessionKey;

            _telemetryClient.Context.Device.OemName = _manufacturer;
            _telemetryClient.Context.Device.Model = _model;
            _telemetryClient.Context.Device.OperatingSystem = _osName;
        }


        private void GatherDetails()
        {
            _sessionKey = Guid.NewGuid().ToString();
            _userName = Environment.UserName;
            _osName = GetWindowsFriendlyName();

            _version = CurrentExe.GetVersion();
            _application = $"{ Assembly.GetEntryAssembly().GetName().Name} {_version}";
            _manufacturer = (from x in
                new ManagementObjectSearcher("SELECT Manufacturer FROM Win32_ComputerSystem").Get()
                    .OfType<ManagementObject>()
                             select x.GetPropertyValue("Manufacturer")).FirstOrDefault()?.ToString() ?? "Unknown";
            _model = (from x in
                new ManagementObjectSearcher("SELECT Model FROM Win32_ComputerSystem").Get()
                    .OfType<ManagementObject>()
                      select x.GetPropertyValue("Model")).FirstOrDefault()?.ToString() ?? "Unknown";
        }


        /// <summary>
        /// Retrieve the Windows friendly name instead of just a version
        /// </summary>
        /// <returns></returns>
        private string GetWindowsFriendlyName()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().OfType<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();

            return name?.ToString() ?? Environment.OSVersion.ToString();
        }
    }
}
