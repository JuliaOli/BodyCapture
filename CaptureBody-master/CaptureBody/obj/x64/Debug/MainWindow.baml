   M S B A M L   `   `   `  ����F  BCaptureBody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:CaptureBodyCaptureBody  5   	       CaptureBody.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .controlsclr-namespace:CaptureBody     5   	   -   5          � Title$  
Body Track��5	   	   $	��900��6   $
��1400��6)    � 	Resources 5
         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  UploadButton �� 
TargetType �� Orientation ��Name	 �� TextAlignment
 ��Name �� Triggers P� Property P� Value �� 
TargetName  ScreenshotButton  
PlayButton��%�
  &	        &	 #     &	 F     '�� i     &	 �     7  7  &	      7  7  &	 ^    7  7  �� 5   
   $ 	#ff6e6e6e��62   �� 5   
   $ 	#ff52318f��64   �� 5   
   $ 	#ff00BCF2��62   �� 5   
   " ��6   .�5      ��  �@J�SnapsToDevicePixels . 64   5      �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $ 
Horizontal=�6)   
������ .+�5   "   �� $ 
UploadText��6,   #��z��6@   $
	 Left=�6a   $��Center=�6w   8��  $��15(�6�   � 5   "   $��9,0,0,0q�6'   .+�5   &   �� $
 UploadNormal��6,   $��src\uploadpurple.png��6B   $
��None=�6`   $��Center=�6o   �� 5   &   $
 UploadHover��6,   $��src\uploadblue.png��6A   $
��None=�6]   $��Center=�6l   $y�	Collapsed=�6�   5   #   5      5       5      P� 5       �@J�IsMouseOver . . �5    "   �� $ UploadNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5!   "   $ UploadHover��6O    � y�$ Visible=�6?   6O   �� 5"   "   8  $ 
UploadText��6h    � ��5#      5%      5'      5(      �� 5)   
   " ��6   .�5*      ��  � �� 5+      �� 5,      " ��6&   .R�5-      � .+�5.      �� $ 
Horizontal=�6)   
������ .+�5/   "   �� $ ScreenshotText��6,   #��z��6D   $
	 Left=�6e   $��Center=�6{   8��  $��15(�6�   � 50   "   $��9,0,0,0q�6'   .+�51   &   �� $
 ScreenshotNormal��6,   $��src\ScreenshotNormal.png��6F   $
��None=�6h   $��Center=�6w   �� 52   &   $
 ScreenshotHover��6,   $��src\ScreenshotHover.png��6E   $
��None=�6f   $��Center=�6u   $y�	Collapsed=�6�   53   #   54      55       56      P� 57       �@J�IsMouseOver . . �58   "   �� $ ScreenshotNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 59   "   $ ScreenshotHover��6O    � y�$ Visible=�6?   6O   �� 5:   "   8  $ ScreenshotText��6h    � ��5;      5=      5?      5@      �� 5A   
   " ��6   .�5B      ��  � �� 5C      �� 5D      " ��6&   .R�5E      � .+�5F      �� $ 
Horizontal=�6)   
������ .+�5G   "   �� $ PlayText��6,   #��z��6>   $
	 Left=�6_   $��Center=�6u   8��  $��15(�6�   � 5H   "   $��9,0,0,0q�6'   .+�5I   &   �� $
 
PlayNormal��6,   $��src\run.png��6@   $
��None=�6U   $��Center=�6d   �� 5J   &   $
 	PlayHover��6,   $��src\runblue.png��6?   $
��None=�6X   $��Center=�6g   $y�	Collapsed=�6�   5K   #   5L      5M       5N      P� 5O       �@J�IsMouseOver . . �5P   "   �� $ 
PlayNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5Q   "   $ 	PlayHover��6O    � y�$ Visible=�6?   6O   �� 5R   "   8  $ PlayText��6h    � ��5S      5U      5W      5X      5Y      .��5Z      � $��1��6   $��	0,10,0,10q�6    � ColumnDefinitions 5[   
   �� 5\      $��5* �6   �� 5]      $	��19* �6   .+�5`   
   �� $��0��6   $��30,0,0,0q�6%   $
��Left=�67   $	��250��6R   .+�5a      �� -   5        #��[ 5a      $��Screenshot ��6?   $
��Auto��6U   $
��Left=�6c   $��Center=�6~   $
��0,10q�6�    J� RenderTransformOrigin$ 0.568,5.982&�6�   �� 5b      -   5        #��[ 5b      $��
Play/Pause��69   $��37��6N   $
��Left=�6Z   $��Center=�6u   $
��0,10q�6�   $ 0.568,5.982&�6�   �� 5c      -   5        #��[ 5c      $��Upload      ��6;   $��40��6R   $
��Left=�6^   $��Center=�6y   $
��0,10q�6�   $ 0.568,5.982&�6�   �� 5d      
���� ��$��	Shoulders��6+   $	��150��6>   $	��249��6K   .��5e      �� $
��Left=�6   $	��140��68   $��
0,0,-2,-10q�6E   $	��Top=�6Y   $	��239��6q   .+�5f      �  5g      �� 5h      $��* �6/   �� 5i      $��* �6/   .+�5k      �� -   5         ��Name$ 
ElevationS��5k   #   $��0,5,0,5q�67   $��0��6H   $��Shoulder Elevation��6X   $
��10.5(�6u   �� 5l      -   5        $ InadequateS��5l   #   $��0,5,0,5q�68   $��3��6I   $��Inadequate Posture��6Y   $
��10.5(�6v   5m      �� 5n      -   5        $ tblRightShoulderFlexion��5n   C   
���   �$��0��63   $
��10.5(�6d   $��3q�6t   �� 6�   $4�.Right Shoulder Flexion: N/A       Timer: 00:00��6�   6�   �� 5o      -   5        $ tblLeftShoulderFlexion��5o   3   
���   �$��0��6S   $
��10.5(�6c   $��3q�6s   �� 6�   $6�0Left Shoulder Flexion: N/A          Timer: 00:00��6�   6�   �� 5p      -	   5        $ tblRightShoulderAbduction��5p   3   
���   �$
��10.5(�6V   $��	3,3,-23,3q�6f   �� 6{   $2�,Right Shoulder Abduction: N/A   Timer: 00:00��6   6�   �� 5q      -
   5        $ tblLeftShoulderAbduction��5q   3   
���   �$
��10.5(�6U   $��3q�6e   �� 6r   $3�-Left ShoulderAbduction: N/A      Timer: 00:00��6v   6�   5r      5s      �� 5u      
���� ��$
��Neck��6+   $	��110��69   $	��249��6F   .��5v      �� $
��Left=�6   $	��100��68   $��
0,0,-2,-10q�6E   $	��Top=�6Y   $	��239��6q   .+�5w      �  5x      �� 5y      $��* �6/   �� 5z      $��* �6/   .+�5|      �� -   5        $ SymetryN��5|   #   $��0,5,0,5q�65   $��0��6F   $��Neck Symmetry��6V   $
��10.5(�6n   5}      �� 5~      -   5        $ tblNeckFlexion��5~   3   
���   �$
��10.5(�6K   $��3q�6[   �� 6h   $*�$Neck Flexion: N/A       Timer: 00:00��6l   6�   �� 5      -   5        $ tblNeckExtension��5   3   
���   �$
��10.5(�6M   $��3q�6]   �� 6j   $�Neck Extension: N/A��6n   6�   5�      5�      �� 5�      
���� ��$��Trunk��6+   $��80��6:   $	��250��6F   .��5�      �� $
��Left=�6   $��60��68   $��	0,0,-1,-2q�6D   $	��Top=�6W   $	��239��6o   .+�5�      �  5�      �� 5�      $��* �6/   �� 5�      $��* �6/   .+�5�      �� -   5        $ RectifinedT��5�   #   $��0,5,0,5q�68   $��0��6I   $��Rectified Spine��6Y   $
��10.5(�6s   �� 5�      -   5        $ SymetryT��5�   #   $��0,5,0,5q�65   $��3��6F   $��Trunk Symetry��6V   $
��10.5(�6n   5�      �� 5�      -   5        $ tblTrunk��5�   3   
���   �$
��10.5(�6E   $��3q�6U   �� 6b   $-�'Trunk Flexion: N/A         Timer: 00:00��6f   6�   5�      5�      �� 5�      
���� ��$��	Positions��6+   $	��154��6>   $	��249��6K   .��5�      �� $
��Left=�6   $	��130��68   $��
0,0,-2,-10q�6E   $	��Top=�6Y   $	��239��6q   .+�5�      �� �� 5�      -   5        $ 	tblHeight��5�   3   
���   �$
��10.5(�6F   $��3q�6V   �� 6c   $�He��6g   �� 6s   $�ight: ��6w   �� 6�   $	�N/A��6�   6�   �� 5�      -   5        $��tblLeft��5�   3   
���   �$��Left Hand: N/A��6B   $
��10.5(�6X   $��3q�6h   �� 5�      -   5        $��tblRight��5�   3   
���   �$��Right Hand: N/A��6C   $
��10.5(�6Z   $��3q�6j   �� 5�      -   5        $ tblPositionHeaderX��5�   3   
���   �$
��10.5(�6O   $��3q�6_   �� 6l   $�Posit��6p   �� 6   $�i��6�   �� 6�   $�
on Header ��6�   �� 6�   $�X: N/A��6�   6�   �� 5�      -   5        $��tblPositionHeaderY��5�   3   
���   �$��Position Header Y: N/A��6M   $
��10.5(�6k   $��3q�6{   �� 5�      -   5        $��tblPositionHeaderZ��5�   3   
���   �$��Position Header Z: N/A��6M   $
��10.5(�6k   $��3q�6{   5�      5�      5�      � 5�   
   $��1��6   $��0,0,30,0q�6    � RowDefinitions 5�      �� 5�      $
��0.8* �6    .+�5�      (� .��5�         CaptureBody.KinectViewer  -   5          Name$ viewer��5�   (   $
��2078��68   $
��1857��6E   5�      5�      5�      5�      