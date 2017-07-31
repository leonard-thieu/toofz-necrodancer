USE [SqlBulkUpsertTestDb]
GO
/****** Object:  Table [dbo].[TestUpsert]    Script Date: 05/08/2011 02:35:07 ******/
SET IDENTITY_INSERT [dbo].[TestUpsert] ON
INSERT [dbo].[TestUpsert] ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image], [nullable_xml]) VALUES (N'TEST', 1, N'some text here 1', 11, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TestUpsert] ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image], [nullable_xml]) VALUES (N'TEST', 2, N'some text here 2', 22, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TestUpsert] ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image], [nullable_xml]) VALUES (N'TEST', 3, N'some text here 3', 33, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TestUpsert] ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image], [nullable_xml]) VALUES (N'TEST', 4, N'some text here 4', 44, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TestUpsert] ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image], [nullable_xml]) VALUES (N'TEST', 5, N'some text here 5', 55, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TestUpsert] ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image], [nullable_xml]) VALUES (N'TEST', 6, N'some text here 6', 66, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TestUpsert] ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image], [nullable_xml]) VALUES (N'TEST', 7, N'some text here 7', 77, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TestUpsert] ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image], [nullable_xml]) VALUES (N'TEST', 8, N'some text here 8', 88, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TestUpsert] ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image], [nullable_xml]) VALUES (N'TEST', 9, N'some text here 9', 99, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TestUpsert] ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image], [nullable_xml]) VALUES (N'TEST', 10, N'some text here 10', 110, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[TestUpsert] OFF
