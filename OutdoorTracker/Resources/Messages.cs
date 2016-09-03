﻿//--------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by a tool.
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//--------------------------------------------------------------------------------------------------

#pragma warning disable 1570

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization; 
using System.Runtime.CompilerServices;

using Windows.ApplicationModel.Resources;


namespace OutdoorTracker.Resources
{
    [CompilerGenerated]
    [DebuggerNonUserCode]
    [GeneratedCode("TextTemplatingFileGenerator", "1.0")]
    public static class Messages
    {
        private static readonly ResourceLoader ActiveResourceLoader = ResourceLoader.GetForCurrentView("Messages");

        public static class Dialog
        {
            /// <summary>
            ///      Looks up a Text similar to "Cancel"
            /// </summary>
            public static string Cancel
            {
                get { return ActiveResourceLoader.GetString("Dialog_Cancel"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "No"
            /// </summary>
            public static string No
            {
                get { return ActiveResourceLoader.GetString("Dialog_No"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Ok"
            /// </summary>
            public static string Ok
            {
                get { return ActiveResourceLoader.GetString("Dialog_Ok"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Would you like to report this Error?"
            /// </summary>
            public static string SendReport
            {
                get { return ActiveResourceLoader.GetString("Dialog_SendReport"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Yes"
            /// </summary>
            public static string Yes
            {
                get { return ActiveResourceLoader.GetString("Dialog_Yes"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Yes, send report"
            /// </summary>
            public static string YesSendReport
            {
                get { return ActiveResourceLoader.GetString("Dialog_YesSendReport"); }
            }

        }

        public static class GpxTrackBuilder
        {
            /// <summary>
            ///      Looks up a Text similar to "GPX"
            /// </summary>
            public static string Format
            {
                get { return ActiveResourceLoader.GetString("GpxTrackBuilder_Format"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "There are not Tracks in this File."
            /// </summary>
            public static string NoTracksFound
            {
                get { return ActiveResourceLoader.GetString("GpxTrackBuilder_NoTracksFound"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "The selected tracks have been exported successfully."
            /// </summary>
            public static string Success
            {
                get { return ActiveResourceLoader.GetString("GpxTrackBuilder_Success"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Export completed"
            /// </summary>
            public static string SuccessTitle
            {
                get { return ActiveResourceLoader.GetString("GpxTrackBuilder_SuccessTitle"); }
            }

        }

        public static class KmlTrackBuilder
        {
            /// <summary>
            ///      Looks up a Text similar to "KML"
            /// </summary>
            public static string Format
            {
                get { return ActiveResourceLoader.GetString("KmlTrackBuilder_Format"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "There are not Tracks in this File."
            /// </summary>
            public static string NoTracksFound
            {
                get { return ActiveResourceLoader.GetString("KmlTrackBuilder_NoTracksFound"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "The selected tracks have been exported successfully."
            /// </summary>
            public static string Success
            {
                get { return ActiveResourceLoader.GetString("KmlTrackBuilder_Success"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Export completed"
            /// </summary>
            public static string SuccessTitle
            {
                get { return ActiveResourceLoader.GetString("KmlTrackBuilder_SuccessTitle"); }
            }

        }

        public static class MapDefinitionManager
        {
            /// <summary>
            ///      Looks up a Text similar to "The selected layer definition could not be imported."
            /// </summary>
            public static string ImportError
            {
                get { return ActiveResourceLoader.GetString("MapDefinitionManager_ImportError"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Cannot import map definition"
            /// </summary>
            public static string ImportErrorTitle
            {
                get { return ActiveResourceLoader.GetString("MapDefinitionManager_ImportErrorTitle"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "The selected layer definition could not be imported, it is not valid."
            /// </summary>
            public static string ImportInvalidJson
            {
                get { return ActiveResourceLoader.GetString("MapDefinitionManager_ImportInvalidJson"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Cannot import map definition"
            /// </summary>
            public static string ImportInvalidJsonTitle
            {
                get { return ActiveResourceLoader.GetString("MapDefinitionManager_ImportInvalidJsonTitle"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "There is already a Map Definition with the name '{0}'. Do you want to overwrite it?"
            /// </summary>
            public static string LayerExists(string name)
            {
                return string.Format(CultureInfo.CurrentCulture, ActiveResourceLoader.GetString("MapDefinitionManager_LayerExists"), name);
            }

            /// <summary>
            ///      Looks up a Text similar to "Overwrite"
            /// </summary>
            public static string Overwrite
            {
                get { return ActiveResourceLoader.GetString("MapDefinitionManager_Overwrite"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Skip"
            /// </summary>
            public static string Skip
            {
                get { return ActiveResourceLoader.GetString("MapDefinitionManager_Skip"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "The Map could not be loaded. Unknown layer definition '{0}'."
            /// </summary>
            public static string UnknownConfig(string config)
            {
                return string.Format(CultureInfo.CurrentCulture, ActiveResourceLoader.GetString("MapDefinitionManager_UnknownConfig"), config);
            }

            /// <summary>
            ///      Looks up a Text similar to "The Map could not be loaded. Unknown projection '{0}'."
            /// </summary>
            public static string UnknownProjection(string projection)
            {
                return string.Format(CultureInfo.CurrentCulture, ActiveResourceLoader.GetString("MapDefinitionManager_UnknownProjection"), projection);
            }

        }

        public static class TrackBuilder
        {
            /// <summary>
            ///      Looks up a Text similar to "{0} Import failed"
            /// </summary>
            public static string ImportFailedTitle(string format)
            {
                return string.Format(CultureInfo.CurrentCulture, ActiveResourceLoader.GetString("TrackBuilder_ImportFailedTitle"), format);
            }

        }

        public static class TrackImporter
        {
            /// <summary>
            ///      Looks up a Text similar to ""
            /// </summary>
            public static string ExportError
            {
                get { return ActiveResourceLoader.GetString("TrackImporter_ExportError"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Failed to export tracks"
            /// </summary>
            public static string ExportErrorTitle
            {
                get { return ActiveResourceLoader.GetString("TrackImporter_ExportErrorTitle"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "GPX file"
            /// </summary>
            public static string GpxFile
            {
                get { return ActiveResourceLoader.GetString("TrackImporter_GpxFile"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Could not import any tracks from the selected file. Are you sure that this is a valid file?"
            /// </summary>
            public static string ImportError
            {
                get { return ActiveResourceLoader.GetString("TrackImporter_ImportError"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Failed to import tracks"
            /// </summary>
            public static string ImportErrorTitle
            {
                get { return ActiveResourceLoader.GetString("TrackImporter_ImportErrorTitle"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "KML file"
            /// </summary>
            public static string KmlFile
            {
                get { return ActiveResourceLoader.GetString("TrackImporter_KmlFile"); }
            }

        }

        public static class TrackRecorder
        {
            /// <summary>
            ///      Looks up a Text similar to "Would you like to continue the last Tracking session?"
            /// </summary>
            public static string ContinueTracking
            {
                get { return ActiveResourceLoader.GetString("TrackRecorder_ContinueTracking"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Tracking session found"
            /// </summary>
            public static string ContinueTrackingTitle
            {
                get { return ActiveResourceLoader.GetString("TrackRecorder_ContinueTrackingTitle"); }
            }

        }

    }
}

#pragma warning restore 1570