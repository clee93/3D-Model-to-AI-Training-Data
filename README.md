# 3D-Model-to-AI-Image-Training-Data
Renders 3D model in with various configurations and labels them for AI image training.

## Contents
* [Setup & Requirements](README.md#setup--requirements)
* [Usage](README.md#usage)

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

**Accessing the Render**

![Pic](ReadmeImages/Hierarchy.png?raw=true "Hierarchy")

You can access the render in the Studio.unity scene and select `Render` as shown in highlighed blue and then navigate to the Inspector Window.

![Pic](ReadmeImages/Render.png?raw=true "Render")

** Using the Render **

Everything above the `---------------` line is pre-filled and can be ignored.

* Configuration
   * Render Debug Images
      > This will output an addition png image per every viable training iamge with a bounding box surrounding the object.
* Output Resolution
   * Output Resolution
      > Allows modification to the ouput image so that it may fit the neural net
   * Label Type
   
      | Label                 | Ouput                                                                                                                      | 
      | --------------------- | -------------------------------------------------------------------------------------------------------------------------- |
      | NoLabel               | Images are unlabled                                                                                                        |
      | ResizeToLabel         | Images are cropped so that the object fills the entire screen, then placed in their respective folders                     |
      | YOLOUsingDarknet      | Images are outputed with label and class details as specified for https://github.com/AlexeyAB/darknet                      |
      | YOLOUsingTensorFlow   | Images are outputed with label and class details as specified for https://github.com/pythonlessons/TensorFlow-2.x-YOLOv3   | 
      | PascalVOC             | Not yet implemented                                                                                                        | 
      | Keras                 | Images are exported and placed in their respective folders for https://www.tensorflow.org/tutorials/images/classification  |
      
      
## Additional Resources
**Darknet Tutorial:**

https://www.youtube.com/watch?v=sKDysNtnhJ4

https://www.youtube.com/watch?v=-NEB5P-SLi0

<br/>

**Yolo on Tensorflow Tutorial:**

https://www.youtube.com/watch?v=PaJ1rU-DgLs

