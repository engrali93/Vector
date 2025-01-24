# <span style="color: #4CAF50;">Vector Graphic Viewer</span>

This project is a simple vector graphic viewer built using <strong>C#</strong>, <strong>WPF</strong>, and the <strong>MVVM</strong> design pattern. It reads vector shape data from a JSON file and dynamically displays it in a user-friendly interface. Users can view, interact with, and highlight different shapes, with detailed shape information displayed upon selection.

<br>

## <span style="color: #2196F3;">Key Features:</span>
<ul>
    <li>Supports three types of shapes: <strong>CloseVector</strong>, <strong>Ellipse</strong>, and <strong>OpenVector</strong>.</li>
    <li>Dynamic shape categorization based on properties like <code>Filled</code> and <code>Radius</code>.</li>
    <li>Easy-to-use UI to display shapes with interactive highlighting and detailed data.</li>
    <li>Future scalability to support additional features like zoom, pan, and shape transformations.</li>
</ul>

<br>

## <span style="color: #2196F3;">Folder Structure:</span>
<ul>
    <li><strong>HelperClasses</strong>: Contains logic for shape conversion, loading, and rendering.</li>
    <li><strong>Models</strong>: Contains data classes for each shape type (<code>CloseVector</code>, <code>Ellipse</code>, <code>OpenVector</code>).</li>
    <li><strong>ViewModels</strong>: Manages the state and logic for the UI.</li>
    <li><strong>Views</strong>: Contains the XAML for the main window, where the shapes are displayed.</li>
</ul>

<br>

## <span style="color: #4CAF50;">Future Enhancements:</span>
<p>There are plans to extend the functionality to include features such as:</p>
<ul>
    <li>Shape transformations (rotate, resize, etc.).</li>
    <li>Additional shape types.</li>
    <li>Improved UI for better interactivity.</li>
</ul>

---
