# 3D-Model-to-AI-Image-Training-Data
Renders 3D model in with various configurations and labels them for AI image training.

## Contents
* [Setup & Requirements](README.md#setup--requirements)
* [Usage](README.md#usage)
* [Additional Resources](README.md#additional-resources)
* [Common Problems](README.md#common-problems)

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

**Using the Render**

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
      
* Number of Angles Captured per Axis
   * Horizontal Spin
      > This denotes the number of image(s) that will be captured about the Y axis formulated by `(360/Horizontal Spin)`
   * Horizontal Spin Random Variance
      > This denotes the random variance allowed between 0 and the selected number.  The number selected is in degrees.
   * Vertical Rotation
      > This denotes the number of image(s) that will be captured about the X axis formulated by `(90/Vertical Rotation)`.  The camera pans from ground level and moves upwards directly over head (90 degrees max)
   * Vertical Rotation Random Variance
      > This denotes the random variance allowed between 0 and the selected number.  The number selected is in degrees.
   * Tilt
      > This denotes the number of image(s) that will be captured about the Z axis formulated by `(360/Tilt)`
   * Tilt Random Variance
      > This denotes the random variance allowed between 0 and the selected number.  The number selected is in degrees.
   * Zoom
      > This denotes how far the camera is from the object.  This does not use FOV, but rather translation of the camera.
* Start Render
   > This will start the render.  Note that you must be in playmode for the render to occur.
* Note
   * All Usable Training Images
      > Denotes the number of total images that will be generated.
   * Images Per Model
      > The exact numbers that will be generated per class.
   * Maxium Possible Train Set Size
      > This is the maxium space required for the training set (debug images are not considered). This does not take into account compression. The actual size will most likely be smaller.

**Configuring the Subject and Environment**

![Pic](ReadmeImages/HierarchyExploaded.png?raw=true "Hierarchy Exploaded View")

* Lighting Profiles
   > The render will iterated through each `GameObject` in `Lighting Profiles` (only first layer is affected, no recursion or nesting).
   > 
   > Select `Directional Light` then press `ctrl + d` to add more lighting profiles, move to the inspector to adjust the specific lighting conditions desired.
* Focused Model Folder
   > The render will iterated through each `GameObject` in `FocusedModelFolder` (only first layer is affected, no recursion or nesting).
   > 
   > The user must manually position the Object after dragging it into the `FocusedModelFolder`, this can be done easily by enabling the `COV` `GameObject` as a refernce for centering.
   > 
   > The `class.names` files will take on the name of Object
* Environment Folder
   > The render will iterated through each `GameObject` in `EnvironmentFolder` (only first layer is affected, no recursion or nesting).
   > 
   > Environments are not randomly generated, the user must manually add them.

## Common Problems

**Missing User Layer 11**
* Error
   > ![Pic](ReadmeImages/TagError.png?raw=true "Missing Layer")
* Fix
   > ![Pic](ReadmeImages/Tags.png?raw=true "Tags and Layers")
   > 
   > Add `RenderObject` to UserLayer 11


**Missing Textures**
* Error
   > Model is untextured
* Fix
   > ![Pic](ReadmeImages/Texture.png?raw=true "Texture")
   > 
   > Select `Extract Textures`

## Additional Resources
**Darknet Tutorial:**

https://www.youtube.com/watch?v=sKDysNtnhJ4

https://www.youtube.com/watch?v=-NEB5P-SLi0

<br/>

**Yolo on Tensorflow Tutorial:**

https://www.youtube.com/watch?v=PaJ1rU-DgLs

