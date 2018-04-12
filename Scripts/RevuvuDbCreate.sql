USE master
GO

IF EXISTS(SELECT * FROM sys.databases WHERE name='Revuvu')
DROP DATABASE Revuvu
GO

CREATE DATABASE Revuvu
GO