# Animperium
A general application for creating animated videos, leaning towards a minimalist, yet broadly scoped user-interface.

See ui_docs.md for the general structure and features planned for the application. As with all WPF UI files, UI elements are divided into .xaml and .xaml.cs files.

The UI is divided into the following sections:
- **The Main Window.** (see. MainWindow) The root of the UI.
  - **The Animation Canvas.** (see Controls/AnimationCavas) Where all the fun happens. Basically, the animation itself is observed.
  - **The Tool View.** (see. Controls/TooView) The tools for creating the animations and its objects.
  - **The Item View.** (see. Controls/ItemView) The items that have been added to the animation canvas.
  - **The Property Panel.** (see. Controls/PropertyPanel) The panel where the properties to be animated or set are manipulated. This allows to select a few properties to be shown on the Timeline Property Canvas.
  - **The Timeline Property Panel.** (see Controls/TimelinePropertyCanvas/TimelinePropertyCanvas) Where the values of each property for each given item are controlled, frame by frame. Basically the "timeline" on other applications, but only a few focused properties are addressed at any oen time.
 
This UI is subject to a bit of change and improvement to make it more simplistic. But this covers the general idea of the UI.
