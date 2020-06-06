# Sunburst

Basic procedurally generated sunburst mesh and shader for use in UI applications.

## Usage

Add a 'Sunburst' and 'Canvas Renderer' component to a GameObject underneath a 'Canvas' node. In the inspector you can modify the following fields:

- Segment Count: The number of segments the geometry will be divided into.
- Beam Cap: Can be used to render only the first X segments from the sunburst.
- Segment Width (0-1): This is the ratio of the segment which the beam geometry will take up.
- Uv Mapping Mode:
    - Per Segment: UVs are mapped so that v=0 is at the center of the sunburst, and v=0 is on the outer edge.
    - Uniform: UVs are mapped uniformly to the object.

## Shaders

The package comes with two shaders which are based on the default UI shader

### Sunburst

Cheaper of the two shaders. This shader simply linearly interpolates the alpha towards zero at the outer edge of the radial. Should be used with Per Segment mapping.

### SunburstAntiAliasing

A more expensive version of the shader which renders a feathered edge to the segments to combat aliasing issues. The feather witdth can be controller using the '_Feather' property.