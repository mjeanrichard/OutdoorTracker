# OutdoorTracker
[![Build status](https://ci.appveyor.com/api/projects/status/303xd3yswefv1ik5?svg=true)](https://ci.appveyor.com/project/MJeanRichard/outdoortracker)

[<img src="https://assets.windowsphone.com/f2f77ec7-9ba9-4850-9ebe-77e366d08adc/English_Get_it_Win_10_InvariantCulture_Default.png" alt="Get it on Windows 10" Width=300 />](https://www.microsoft.com/store/apps/9NBLGGH4XD91?ocid=badge)

## Beta Builds
For the latest Beta Builds see here (*NOTE:* To install these builds, you need to enable developer mode on your device):  
[Outdoor Tracker Beta on Appveyor](https://ci.appveyor.com/project/MJeanRichard/outdoortracker/build/artifacts) 


You can install the Beta Version side by side with the Store Version, since they have a different ProductId and Name.

## Adding More Maps

:bangbang: **Please make sure that you have the apropriate licenses before adding more maps to the app** :bangbang: 

The Outdoor Tracker can be extended with more Maps. To add additional Maps to the App a JSON File with the following Structure can be imported into the Application

```JSON
[
    {
        "name": "<MapName>",
        "projection": "<Projection>"
        "layers": [
            {
                "name": "<LayerName>",
			    "config": "<ConfigType>",
			    "parameters": 
			    {
		    		"urlPattern": "<UrlPattern>"
    			}
            }
        ]
    },
    ...
]
```

| Parameter  | Remarks   |
| :--- | :--- |
| `MapName`    | The Name of the Map. This name is displayed to the user and used as a uniqe identification for each map. |
| `Projection` | Either `WebMercator` or `SwissGrid`. Webmercator is used for almost all available online maps such as OpenStreetMaps, Google, etc. SwissGrid should only be used with the maps from SwissTopo (http://maps.geo.admin.ch) |
| `LayerName`  | The name of the Layer. Currently the App suports only one Layer per map. Make sure this name is uniquq, since it is used for caching the tiles. |
| `ConfigType` | Either `Default` or `SwissTopo`. Use Default exept for maps from SwissTopo (together with the SwissGrid projection). |
| `UrlPattern` | Only used with ConfigType `Default`, see the following Table.  |

For the URL Pattern you can use the following Placeholders, that will be replaces when downloading the tiles:
| Placeholder     | Remarks |
| :---             | :--- |
| `{x}`           | x-Index of the map tile. |
| `{y}`           | y-Index of the map tile. |
| `{z}`           | z-Index of the map tile. |
| `{RND-1;2;...}` | This pattern is replaced with one (selected at random) of the values after the `RND-` token. The values are separated with a `;`. |

Examples:
* `http://mt{RND-1;2;3}.google.com/vt/lyrs=m&x={x}&y={y}&z={z}`
* `http://{RND-a;b;c}.tile.openstreetmap.org/{z}/{x}/{y}.png`

You can find other examples in the Layers folder of the Project.

## Release Notes
### [Milestone 1.1](../../milestone/2?closed=1)
#### Version 1.1.9 (public store)
- Fixed unavailable location data

#### Version 1.1.9 (public store)
- Fixed Crash on first startup ([#29](../../issues/29))

#### Version 1.1.8 (public store)
- Fixed crash when the current track is deleted ([#26](../../issues/26))
- Fixed AccessViolation on Startup ([#28](../../issues/28)
- Fixed exception when a track is resumed, without existing track points ([#27](../../issues/27))

#### Version 1.1.7 (public store)
- Fixed crash on resume while tracking ([#23](../../issues/23))
- Fixed crashes when the Database does not exist
- Fixed crash when a layer cannot be imported ([#22](../../issues/22))
- Improved error reporting

#### Version 1.1.6 (public store)
- Added Clear Cache Function
- Fixed Accuracy Circle
- Fixed Map Cache
- Fixed Null Reference Exception in TrackBuilder ([#20](../../issues/20))
- Improved UI

#### Version 1.1.5 (public store)
- Fixed Crash when BatchSprites are not supported ([#18](../../issues/18))
- Fixed Crash because the Database was not initialized correctly ([#17](../../issues/17))

#### Version 1.1.4 (public store)
- Fixed Translation Bug
- Added Setting to change Language

#### Version 1.1.3 (public store)
- Added Privacy Policy ([#15](../../issues/15))
- Improved Performance
- Added Translation for German

#### Version 1.1.2 
- Added ETW Logging ([#2](../../issues/2))
- Display busy state on all views ([#11](../../issues/11))

#### Version 1.1.1 (Code required)
- Added About Page ([#10](../../issues/10))
- Fixed several small bugs ([#1](../../issues/1), [#9](../../issues/9))
- Added Configuration for Track smoothing ([#8](../../issues/8))

### [Milestone 1.0](../../milestone/1?closed=1)
#### Version 1.0.0 (Code required)
- Initial release
- OpenStreetMap
- Basic Tracking
- Import GPX / KML
