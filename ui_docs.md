# Animperium

This readme is for developers working on the project.

The purpose of this project is to build a desktop animation application for creating videos, in particular with:

1. A semi-minimalist user interface: A clean layout where all the features are laid out clearly.
2. A gradual learning curve for its use: So new users going into animation can adapt easily.
3. Features targeted towards education, to make it significantly easier to produce educational content.

## Process

1. Tool selected from **Tool View**. 
2. Tool draw item on **Canvas** (if it’s a visual tool). Item is then selected. (Audio to be implemented)
3. Item added to canvas is added to the **Item View**. 
4. Properties of the selected item are then edited in the **Property View**.
5. Properties can be set to be animatable in the property view. 
6. Animatable properties can be edited on the **Timeline**. 

## Main Components

### Tools

All interaction on the application begins with the **Tool View**, which contains the tools. Tools are divided into 2 types:

* **Visual Tools.** These tools pertain to what is *seen* on the video. They affect the Canvas.
* **Auditory Tools.** These tools pertain to what is *heard* on the video. They affect the Audio Bar.

Most tools will be visual.

On top of the tool view, you have the tools being divided into the following **Tool Groups**:

1. **Cursors.** These tools revolve around selections and their transformations (scaling, shifting, rotating, shearing), and modifying vector nodes.
2. **Curves.** These are tools for creating curves via curve interpolations. They allow for various customized shapes and objects.
3. **Shapes.** These tools create all sorts of basic shapes with a couple of parameters that allow for easier modifications.
4. **Text.** This allows for the writing of various types of text onto the canvas. (Possible future implementation: Automated speech may be included here.)
5. **Media.** This tool group imports images, video, and audio. It can also contain predefined media for custom use. The media can be added for a specific file, or be treated as part of the application so it may be used in future files. (Recording media may need to be implemented in the future.)
6. **Effects.** This tool group is for producing new objects or modifying selected data (visual objects or audio). This includes features such as offsets for vectors, drop shadows for images, visual effects for animations, and noise reduction for audio. If nothing is selected, effects can be used to manipulate all objects or the Canvas itself (which is selected by default).
   * Mesh warp: Allows gradient warping of a mesh.
     * Raster: Distorts and reshapes the image.
     * Vector: Produces coloured overlay, via a gradient mesh warp.

### Items

Once a tool with its given parameters is selected, it interacts primarily the **Canvas**. Curve, Shape, Visual Text, and Visual Media tools all create new items on the **canvas**, and are called **visual items**. The canvas itself is also a visual item. Cursors selected and manipulate existing items directly, in simple and limited ways. Effects do so in advanced ways that requires additional code. Effects are also considered items. In contrast to visual items, **auditory items** are audio.

Once an item has been created, it is selected and added to the **Item View**. The item view is first divided into visual vs auditory, then  organizes the existing items into **layers**.

Layer features include:

* **Clipping.** 
* **Blend modes.** 
* **Property Pairing.** Properties of different items can be paired. Alternatively, one property can be used to pair with another of the same item.
* **Combine FX.** Same effect as combining curves, but as an animation.

### Animation

A selected item can then have its properties manipulated in the **Property View**. This will affect its various features, such size and colour. Properties can be set as animatable. If they are, they can be manipulated and changed over time via the **Timeline**.

## Additional Tools

### Placement

Setting affects how objects are placed

* On Top
* On Bottom
* (Below Selected)
* (Above Selected)
* (Inside Selected)

### Simple Transformations

* Rotation by +/- 90 or 180 degrees. (3 operations - rotation by 180 is same both ways)
* Flips across horizontal, vertical, or diagonal axes. (4 operations)

### Combine

Combine tools combine 2 or more curves to produce new vectors.

* Add
* Subtract
* Intersect
* Unify
* Divide

### Alignment

* Align or Space
* Horizontal or Vertical or Both
* Left/Middle/Right or Top/Middle/Bottom or NW/SW/O/NE/SE, respectively.
* If Space, Gaps option included
  * For Both, Gaps = X/Y/Forward Diagonal/Back Diagonal/Origin
* Relative to Selected Items or to Canvas
* (Apply Alignment Button)

### Snapping

* 
