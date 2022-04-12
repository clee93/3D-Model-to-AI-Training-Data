# 3D-Model-to-AI-Image-Training-Data
Renders 3D model in with various configurations and labels them for AI image training.

[test](README.md#setup--requirements)

## Setup & Requirements
**Requirements**
- Unity Engine (tested on 2020.3.30f1)

<br/>

**Setup**
1. Create a unity project with the HDRP template

2. Copy over folders into their respective directories
    * i.e. move the content of the Assets in the github repository into Unity's Assets folder
3. Navigate to ./Assets/VirtualRenderStudio/Scenes and open Studio.unity

<br/>

**Modifying to Work Without HDRP**
- HDRP is required, however you can bypass this restriction by removing all lines that include `//REMOVE THIS LINE FOR NON-HDR`
https://github.com/clee93/3D-Model-to-AI-Training-Data/blob/410fe435b794fa8b0d0ab9ac3f35dca7fd07cdc8/Assets/VirtualRenderStudio/Scripts/Render.cs#L9
https://github.com/clee93/3D-Model-to-AI-Training-Data/blob/410fe435b794fa8b0d0ab9ac3f35dca7fd07cdc8/Assets/VirtualRenderStudio/Scripts/Render.cs#L27
https://github.com/clee93/3D-Model-to-AI-Training-Data/blob/410fe435b794fa8b0d0ab9ac3f35dca7fd07cdc8/Assets/VirtualRenderStudio/Scripts/Render.cs#L306
https://github.com/clee93/3D-Model-to-AI-Training-Data/blob/410fe435b794fa8b0d0ab9ac3f35dca7fd07cdc8/Assets/VirtualRenderStudio/Scripts/Render.cs#L313

## Usage
