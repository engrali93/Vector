   M S B A M L   `   `   `  ����E  AVectorDraw, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:VectorDraw.ViewModelsVectorDraw.ViewModels  5   	   %    VectorDraw.Views.MainWindowView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:VectorDraw.ViewModels     5   	     � Title$  MainWindowView��5   	    � WindowState$ 	Maximized=�6    $	��450��68   $	��800��6E    � DataContext 5	      /   )VectorDraw.ViewModels.MainWindowViewModel  5
   
   .��5      � 
������� � ColumnDefinitions 5   
   �� 5      $	��200 �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��1��6    �� Orientation$ 
Horizontal=�6%   $��Center=�6>   $��Center=�6[   $��10q�6v   .+�5      �� $��Load JSON File��6   $	��150��6.   $��30��6:   $
��Left=�6F   $	��Top=�6a   $��	0,0,100,0q�6y   ��6�   �� *LoadJsonCommand+�� 5      $��Zoom In��6   $��75��6-   $��30��68   ��6D   �� *ZoomInCommand+�� 5      $��Zoom Out��6   $��75��6.   $��30��69   $��10,0,0,0q�6h   ��6E   �� *ZoomOutCommand+6h   5      �� 5"   
   $��0��6   $��0��5#   
   $��2��5$   
    �� DisplayMemberPath$
 Type��5'   
   $��5q�5(   
   $��Stretch=�5)   
   $��Stretch=�5*   
   
�������$��1q�5,   
   
���������5%   
   �� *	ShapeList+ �� SelectedItem 5&   
   �� *SelectedShape+5.      �� 51   
   -   5        
������ $��2��57      $��1��6"   $��Stretch=�62   ��56      �� *CanvasWidth+��6/   �� *CanvasHeight+z�58      V� 59      .~�5:      ��  �� ScaleX 6%   �� *	zoomLevel+	 �� ScaleY	 6B   �� *	zoomLevel+T� 5;      
 T� X
 6)   �� *	OffsetX+ �� FallbackValue$ 0�� T� Y 6P   �� *	OffsetY+$ 0��5<      .+�5>      �� ��6   �� *CanvasElements+��5?      �� 5@      .R�5A      �� 5B      5D      5E      �� 5G   
   $��2��6   $��2��5H      $��5q�5J      $��Stretch=�5K      $��Stretch=�5L      
������� �� Padding$ 5q�5N      ��5I      �� *SelectedShapeInfo+5N      5O      5P      