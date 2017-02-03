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

        public static void Init()
        {
            // Do Nothing
        }

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
            ///      Looks up a Text similar to "Create Issue"
            /// </summary>
            public static string GotoGithub
            {
                get { return ActiveResourceLoader.GetString("Dialog_GotoGithub"); }
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
            ///      Looks up a Text similar to "Could not find any Tracks in this file."
            /// </summary>
            public static string NoTracksFound
            {
                get { return ActiveResourceLoader.GetString("GpxTrackBuilder_NoTracksFound"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "If you think this is an error, please create an issue on GitHub."
            /// </summary>
            public static string NoTracksFoundMessage
            {
                get { return ActiveResourceLoader.GetString("GpxTrackBuilder_NoTracksFoundMessage"); }
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
            ///      Looks up a Text similar to "If you think this is an error, please create an issue on GitHub."
            /// </summary>
            public static string NoTracksFoundMessage
            {
                get { return ActiveResourceLoader.GetString("KmlTrackBuilder_NoTracksFoundMessage"); }
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

        public static class LayersViewModel
        {
            /// <summary>
            ///      Looks up a Text similar to "Do you want to delete all downloaded maps from this app?"
            /// </summary>
            public static string DeleteCacheMessage
            {
                get { return ActiveResourceLoader.GetString("LayersViewModel_DeleteCacheMessage"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Delete map cache?"
            /// </summary>
            public static string DeleteCacheMessageTitle
            {
                get { return ActiveResourceLoader.GetString("LayersViewModel_DeleteCacheMessageTitle"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Importing Map Layers..."
            /// </summary>
            public static string ImportingMessage
            {
                get { return ActiveResourceLoader.GetString("LayersViewModel_ImportingMessage"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Loading size..."
            /// </summary>
            public static string LoadingSize
            {
                get { return ActiveResourceLoader.GetString("LayersViewModel_LoadingSize"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Size: {0:N0} mb"
            /// </summary>
            public static string SizeText(ulong size)
            {
                return string.Format(CultureInfo.CurrentCulture, ActiveResourceLoader.GetString("LayersViewModel_SizeText"), size);
            }

            /// <summary>
            ///      Looks up a Text similar to "Size not available."
            /// </summary>
            public static string UnknownSize
            {
                get { return ActiveResourceLoader.GetString("LayersViewModel_UnknownSize"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Yes, delete"
            /// </summary>
            public static string YesDelete
            {
                get { return ActiveResourceLoader.GetString("LayersViewModel_YesDelete"); }
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

            /// <summary>
            ///      Looks up a Text similar to "The Element '{0}' is not a known track type."
            /// </summary>
            public static string UnknownType(string elementName)
            {
                return string.Format(CultureInfo.CurrentCulture, ActiveResourceLoader.GetString("TrackImporter_UnknownType"), elementName);
            }

            /// <summary>
            ///      Looks up a Text similar to "Unknown Track Type"
            /// </summary>
            public static string UnknownTypeTitle
            {
                get { return ActiveResourceLoader.GetString("TrackImporter_UnknownTypeTitle"); }
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

        public static class TracksPage
        {
            /// <summary>
            ///      Looks up a Text similar to "Are you sure that you want to delete the selected tracks."
            /// </summary>
            public static string DeleteMessage
            {
                get { return ActiveResourceLoader.GetString("TracksPage_DeleteMessage"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Are you sure that you want to delete the track '{0}'?"
            /// </summary>
            public static string DeleteSingleMessage(string trackName)
            {
                return string.Format(CultureInfo.CurrentCulture, ActiveResourceLoader.GetString("TracksPage_DeleteSingleMessage"), trackName);
            }

            /// <summary>
            ///      Looks up a Text similar to "Delete track"
            /// </summary>
            public static string DeleteSingleTitle
            {
                get { return ActiveResourceLoader.GetString("TracksPage_DeleteSingleTitle"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Delete tracks"
            /// </summary>
            public static string DeleteTitle
            {
                get { return ActiveResourceLoader.GetString("TracksPage_DeleteTitle"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Deleting selected tracks..."
            /// </summary>
            public static string DeletingText
            {
                get { return ActiveResourceLoader.GetString("TracksPage_DeletingText"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Exporting selected tracks..."
            /// </summary>
            public static string ExportingText
            {
                get { return ActiveResourceLoader.GetString("TracksPage_ExportingText"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Importing tracks..."
            /// </summary>
            public static string ImportingText
            {
                get { return ActiveResourceLoader.GetString("TracksPage_ImportingText"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "You are already recoring a track. Do you want to stop recording and create a new track?"
            /// </summary>
            public static string StopTracking
            {
                get { return ActiveResourceLoader.GetString("TracksPage_StopTracking"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Yes, delete"
            /// </summary>
            public static string YesDelete
            {
                get { return ActiveResourceLoader.GetString("TracksPage_YesDelete"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Yes, stop tracking"
            /// </summary>
            public static string YesStopTracking
            {
                get { return ActiveResourceLoader.GetString("TracksPage_YesStopTracking"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Rebuilding Tracks..."
            /// </summary>
            public static string RebuildText
            {
                get { return ActiveResourceLoader.GetString("TracksPage_RebuildText"); }
            }

        }

        public static class TrackWidth
        {
            /// <summary>
            ///      Looks up a Text similar to "Hairline"
            /// </summary>
            public static string Hairline
            {
                get { return ActiveResourceLoader.GetString("TrackWidth_Hairline"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Medium"
            /// </summary>
            public static string Medium
            {
                get { return ActiveResourceLoader.GetString("TrackWidth_Medium"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Thick"
            /// </summary>
            public static string Thick
            {
                get { return ActiveResourceLoader.GetString("TrackWidth_Thick"); }
            }

            /// <summary>
            ///      Looks up a Text similar to "Thin"
            /// </summary>
            public static string Thin
            {
                get { return ActiveResourceLoader.GetString("TrackWidth_Thin"); }
            }

        }

    }
}

#pragma warning restore 1570