   M S B A M L   `   `   `  ���� F  BCaptureBody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:CaptureBodyCaptureBody      CaptureBody.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .controlsclr-namespace:CaptureBody     -     � Title$  
Body Track��$	��900��$
��1400�� � 	Resources    MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  UploadButton �� 
TargetType �� Orientation ��Name	 �� TextAlignment
 ��Name �� Triggers P� Property P� Value �� 
TargetName  ScreenshotButton  
PlayButton��%�  &	        &	      &	 *     '�� ?     &	 t     7  7  &	 n    7  7  &	 �    7  7  �� $ 	#ff6e6e6e���� $ 	#ff52318f���� $ 	#ff00BCF2���� " ��.���  �@J�SnapsToDevicePixels . �� " ��.���  � �� �� " ��.R�� .+��� $ 
Horizontal=�
������ .+��� $ 
UploadText��#��z��$
	 Left=�$��Center=�8��  $��15(�� $��9,0,0,0q�.+��� $
 UploadNormal��$��src\uploadpurple.png��$
��None=�$��Center=��� $
 UploadHover��$��src\uploadblue.png��$
��None=�$��Center=�$y�	Collapsed=� P�  �@J�IsMouseOver . . ��� $ UploadNormal�� � y�$ 	Collapsed=��� $ UploadHover�� � y�$ Visible=��� 8  $ 
UploadText�� � ���� " ��.���  � �� �� " ��.R�� .+��� $ 
Horizontal=�
������ .+��� $ ScreenshotText��#��z��$
	 Left=�$��Center=�8��  $��15(�� $��9,0,0,0q�.+��� $
 ScreenshotNormal��$��src\ScreenshotNormal.png��$
��None=�$��Center=��� $
 ScreenshotHover��$��src\ScreenshotHover.png��$
��None=�$��Center=�$y�	Collapsed=� P�  �@J�IsMouseOver . . ��� $ ScreenshotNormal�� � y�$ 	Collapsed=��� $ ScreenshotHover�� � y�$ Visible=��� 8  $ ScreenshotText�� � ���� " ��.���  � �� �� " ��.R�� .+��� $ 
Horizontal=�
������ .+��� $ PlayText��#��z��$
	 Left=�$��Center=�8��  $��15(�� $��9,0,0,0q�.+��� $
 
PlayNormal��$��src\run.png��$
��None=�$��Center=��� $
 	PlayHover��$��src\runblue.png��$
��None=�$��Center=�$y�	Collapsed=� P�  �@J�IsMouseOver . . ��� $ 
PlayNormal�� � y�$ 	Collapsed=��� $ 	PlayHover�� � y�$ Visible=��� 8  $ PlayText�� � ��.��� $��1��$��	0,10,0,10q� � ColumnDefinitions �� $��5* ��� $	��19* �.+��� $��0��$��30,0,0,0q�$
��Left=�$	��250��.+��� -   #��[ $��Screenshot ��$
��Auto��$
��Left=�$��Center=�$
��0,10q� J� RenderTransformOrigin$ 0.568,5.982&��� -   #��[ $��
Play/Pause��$��37��$
��Left=�$��Center=�$
��0,10q�$ 0.568,5.982&��� -   #��[ $��Upload      ��$��40��$
��Left=�$��Center=�$
��0,10q�$ 0.568,5.982&��� 
���� ��$��	Shoulders��$	��150��$	��249��.���� $
��Left=�$	��140��$��
0,0,-2,-10q�$	��Top=�$	��239��.+��  �� $��* ��� $��* �.+��� -    ��Name$ 
ElevationS��$��0,5,0,5q�$��0��$��Shoulder Elevation��$
��10.5(��� -   $ InadequateS��$��0,5,0,5q�$��3��$��Inadequate Posture��$
��10.5(��� -   $ tblRightShoulderFlexion��
���   �$��0��$
��10.5(�$��3q��� $4�.Right Shoulder Flexion: N/A       Timer: 00:00���� -   $ tblLeftShoulderFlexion��
���   �$��0��$
��10.5(�$��3q��� $6�0Left Shoulder Flexion: N/A          Timer: 00:00���� -	   $ tblRightShoulderAbduction��
���   �$
��10.5(�$��	3,3,-23,3q��� $2�,Right Shoulder Abduction: N/A   Timer: 00:00���� -
   $ tblLeftShoulderAbduction��
���   �$
��10.5(�$��3q��� $3�-Left ShoulderAbduction: N/A      Timer: 00:00���� 
���� ��$
��Neck��$	��110��$	��249��.���� $
��Left=�$	��100��$��
0,0,-2,-10q�$	��Top=�$	��239��.+��  �� $��* ��� $��* �.+��� -   $ SymetryN��$��0,5,0,5q�$��0��$��Neck Symmetry��$
��10.5(��� -   $ tblNeckFlexion��
���   �$
��10.5(�$��3q��� $*�$Neck Flexion: N/A       Timer: 00:00���� -   $ tblNeckExtension��
���   �$
��10.5(�$��3q��� $�Neck Extension: N/A���� 
���� ��$��Trunk��$��80��$	��250��.���� $
��Left=�$��60��$��	0,0,-1,-2q�$	��Top=�$	��239��.+��  �� $��* ��� $��* �.+��� -   $ RectifinedT��$��0,5,0,5q�$��0��$��Rectified Spine��$
��10.5(��� -   $ SymetryT��$��0,5,0,5q�$��3��$��Trunk Symetry��$
��10.5(��� -   $ tblTrunk��
���   �$
��10.5(�$��3q��� $-�'Trunk Flexion: N/A         Timer: 00:00���� 
���� ��$��	Positions��$	��154��$	��249��.���� $
��Left=�$	��130��$��
0,0,-2,-10q�$	��Top=�$	��239��.+��� �� -   $ 	tblHeight��
���   �$
��10.5(�$��3q��� $�He���� $�ight: ���� $	�N/A���� -   $��tblLeft��
���   �$��Left Hand: N/A��$
��10.5(�$��3q��� -   $��tblRight��
���   �$��Right Hand: N/A��$
��10.5(�$��3q��� -   $ tblPositionHeaderX��
���   �$
��10.5(�$��3q��� $�Posit���� $�i���� $�
on Header ���� $�X: N/A���� -   $��tblPositionHeaderY��
���   �$��Position Header Y: N/A��$
��10.5(�$��3q��� -   $��tblPositionHeaderZ��
���   �$��Position Header Z: N/A��$
��10.5(�$��3q�� $��1��$��0,0,30,0q� � RowDefinitions �� $
��0.8* �.+�(� .��   CaptureBody.KinectViewer  -     Name$ viewer��$
��2078��$
��1857��