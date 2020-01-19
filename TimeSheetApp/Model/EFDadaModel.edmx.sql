
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/19/2020 23:07:19
-- Generated from EDMX file: C:\Users\iliya\source\repos\TimeSheet\TimeSheetApp\Model\EFDadaModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TimeSheetDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_DepartmentsAnalytic]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Analytic] DROP CONSTRAINT [FK_DepartmentsAnalytic];
GO
IF OBJECT_ID(N'[dbo].[FK_DirectionsAnalytic]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Analytic] DROP CONSTRAINT [FK_DirectionsAnalytic];
GO
IF OBJECT_ID(N'[dbo].[FK_UpravlenieTableAnalytic]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Analytic] DROP CONSTRAINT [FK_UpravlenieTableAnalytic];
GO
IF OBJECT_ID(N'[dbo].[FK_OtdelTableAnalytic]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Analytic] DROP CONSTRAINT [FK_OtdelTableAnalytic];
GO
IF OBJECT_ID(N'[dbo].[FK_PositionsAnalytic]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Analytic] DROP CONSTRAINT [FK_PositionsAnalytic];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleTableAnalytic]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Analytic] DROP CONSTRAINT [FK_RoleTableAnalytic];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientWaysTimeSheetTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TimeSheetTable] DROP CONSTRAINT [FK_ClientWaysTimeSheetTable];
GO
IF OBJECT_ID(N'[dbo].[FK_FormatsTimeSheetTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TimeSheetTable] DROP CONSTRAINT [FK_FormatsTimeSheetTable];
GO
IF OBJECT_ID(N'[dbo].[FK_AnalyticTimeSheetTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TimeSheetTable] DROP CONSTRAINT [FK_AnalyticTimeSheetTable];
GO
IF OBJECT_ID(N'[dbo].[FK_ProcessBlock]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Process] DROP CONSTRAINT [FK_ProcessBlock];
GO
IF OBJECT_ID(N'[dbo].[FK_SubBlockProcess]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Process] DROP CONSTRAINT [FK_SubBlockProcess];
GO
IF OBJECT_ID(N'[dbo].[FK_ProcessTypeProcess]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Process] DROP CONSTRAINT [FK_ProcessTypeProcess];
GO
IF OBJECT_ID(N'[dbo].[FK_ResultProcess]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Process] DROP CONSTRAINT [FK_ResultProcess];
GO
IF OBJECT_ID(N'[dbo].[FK_ProcessTimeSheetTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TimeSheetTable] DROP CONSTRAINT [FK_ProcessTimeSheetTable];
GO
IF OBJECT_ID(N'[dbo].[FK_riskChoiseTimeSheetTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TimeSheetTable] DROP CONSTRAINT [FK_riskChoiseTimeSheetTable];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskriskChoise]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[riskChoise] DROP CONSTRAINT [FK_RiskriskChoise];
GO
IF OBJECT_ID(N'[dbo].[FK_TimeSheetTablesupportChoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TimeSheetTable] DROP CONSTRAINT [FK_TimeSheetTablesupportChoice];
GO
IF OBJECT_ID(N'[dbo].[FK_supportChoiceSupports]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[supportChoiceSet] DROP CONSTRAINT [FK_supportChoiceSupports];
GO
IF OBJECT_ID(N'[dbo].[FK_EscalationChoiceEscalations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EscalationChoiceSet] DROP CONSTRAINT [FK_EscalationChoiceEscalations];
GO
IF OBJECT_ID(N'[dbo].[FK_TimeSheetTableEscalationChoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TimeSheetTable] DROP CONSTRAINT [FK_TimeSheetTableEscalationChoice];
GO
IF OBJECT_ID(N'[dbo].[FK_BusinessBlockBusinessBlockChoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BusinessBlockChoiceSet] DROP CONSTRAINT [FK_BusinessBlockBusinessBlockChoice];
GO
IF OBJECT_ID(N'[dbo].[FK_TimeSheetTableBusinessBlockChoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TimeSheetTable] DROP CONSTRAINT [FK_TimeSheetTableBusinessBlockChoice];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Analytic]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Analytic];
GO
IF OBJECT_ID(N'[dbo].[RiskSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RiskSet];
GO
IF OBJECT_ID(N'[dbo].[TimeSheetTable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TimeSheetTable];
GO
IF OBJECT_ID(N'[dbo].[Block]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Block];
GO
IF OBJECT_ID(N'[dbo].[BusinessBlock]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BusinessBlock];
GO
IF OBJECT_ID(N'[dbo].[ClientWays]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientWays];
GO
IF OBJECT_ID(N'[dbo].[Departments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Departments];
GO
IF OBJECT_ID(N'[dbo].[DirectionsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectionsSet];
GO
IF OBJECT_ID(N'[dbo].[Escalations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Escalations];
GO
IF OBJECT_ID(N'[dbo].[ForceQuit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ForceQuit];
GO
IF OBJECT_ID(N'[dbo].[Formats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Formats];
GO
IF OBJECT_ID(N'[dbo].[OtdelTable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OtdelTable];
GO
IF OBJECT_ID(N'[dbo].[Positions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Positions];
GO
IF OBJECT_ID(N'[dbo].[Process]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Process];
GO
IF OBJECT_ID(N'[dbo].[ProcessType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProcessType];
GO
IF OBJECT_ID(N'[dbo].[Result]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Result];
GO
IF OBJECT_ID(N'[dbo].[riskChoise]', 'U') IS NOT NULL
    DROP TABLE [dbo].[riskChoise];
GO
IF OBJECT_ID(N'[dbo].[RoleTable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleTable];
GO
IF OBJECT_ID(N'[dbo].[SubBlock]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubBlock];
GO
IF OBJECT_ID(N'[dbo].[Supports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Supports];
GO
IF OBJECT_ID(N'[dbo].[UpravlenieTable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UpravlenieTable];
GO
IF OBJECT_ID(N'[dbo].[supportChoiceSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[supportChoiceSet];
GO
IF OBJECT_ID(N'[dbo].[EscalationChoiceSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EscalationChoiceSet];
GO
IF OBJECT_ID(N'[dbo].[BusinessBlockChoiceSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BusinessBlockChoiceSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Analytic'
CREATE TABLE [dbo].[Analytic] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [userName] nvarchar(max)  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [FatherName] nvarchar(max)  NULL,
    [DepartmentsId] int  NOT NULL,
    [DirectionsId] int  NOT NULL,
    [UpravlenieTableId] int  NOT NULL,
    [OtdelTableId] int  NOT NULL,
    [PositionsId] int  NOT NULL,
    [RoleTableId] int  NOT NULL
);
GO

-- Creating table 'RiskSet'
CREATE TABLE [dbo].[RiskSet] (
    [id] int IDENTITY(1,1) NOT NULL,
    [riskName] nvarchar(50)  NULL
);
GO

-- Creating table 'TimeSheetTable'
CREATE TABLE [dbo].[TimeSheetTable] (
    [id] int IDENTITY(1,1) NOT NULL,
    [timeStart] datetime  NOT NULL,
    [timeEnd] datetime  NOT NULL,
    [TimeSpent] int  NOT NULL,
    [comment] nvarchar(max)  NULL,
    [Subject] nvarchar(max)  NULL,
    [ClientWaysId] int  NOT NULL,
    [EscalationChoiceId] int  NOT NULL,
    [FormatsId] int  NOT NULL,
    [SupportChoiceId] int  NOT NULL,
    [AnalyticId] int  NOT NULL,
    [Process_id] int  NOT NULL,
    [riskChoise_id] int  NOT NULL,
    [EscalationChoice_id] int  NOT NULL,
    [BusinessBlockChoice_id] int  NOT NULL,
    [supportChoice_id] int  NOT NULL
);
GO

-- Creating table 'Block'
CREATE TABLE [dbo].[Block] (
    [id] int IDENTITY(1,1) NOT NULL,
    [blockName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'BusinessBlock'
CREATE TABLE [dbo].[BusinessBlock] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BusinessBlockName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ClientWays'
CREATE TABLE [dbo].[ClientWays] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Departments'
CREATE TABLE [dbo].[Departments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DirectionsSet'
CREATE TABLE [dbo].[DirectionsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Escalations'
CREATE TABLE [dbo].[Escalations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ForceQuit'
CREATE TABLE [dbo].[ForceQuit] (
    [State] bit  NOT NULL
);
GO

-- Creating table 'Formats'
CREATE TABLE [dbo].[Formats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'OtdelTable'
CREATE TABLE [dbo].[OtdelTable] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Positions'
CREATE TABLE [dbo].[Positions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Process'
CREATE TABLE [dbo].[Process] (
    [id] int IDENTITY(1,1) NOT NULL,
    [procName] nvarchar(max)  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [CommentNeeded] bit  NULL,
    [Block_id] int  NOT NULL,
    [SubBlockId] int  NOT NULL,
    [ProcessType_id] int  NOT NULL,
    [Result_id] int  NOT NULL
);
GO

-- Creating table 'ProcessType'
CREATE TABLE [dbo].[ProcessType] (
    [id] int IDENTITY(1,1) NOT NULL,
    [ProcessTypeName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Result'
CREATE TABLE [dbo].[Result] (
    [id] int IDENTITY(1,1) NOT NULL,
    [resultName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'riskChoise'
CREATE TABLE [dbo].[riskChoise] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Risk_id] int  NULL,
    [Risk_id1] int  NULL,
    [Risk_id2] int  NULL,
    [Risk_id3] int  NULL,
    [Risk_id4] int  NULL,
    [Risk_id5] int  NULL,
    [Risk_id6] int  NULL,
    [Risk_id7] int  NULL,
    [Risk_id8] int  NULL,
    [Risk_id9] int  NULL
);
GO

-- Creating table 'RoleTable'
CREATE TABLE [dbo].[RoleTable] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SubBlock'
CREATE TABLE [dbo].[SubBlock] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [subblockName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Supports'
CREATE TABLE [dbo].[Supports] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UpravlenieTable'
CREATE TABLE [dbo].[UpravlenieTable] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'supportChoiceSet'
CREATE TABLE [dbo].[supportChoiceSet] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Support_id] int  NULL,
    [Support_id1] int  NULL,
    [Support_id2] int  NULL,
    [Support_id3] int  NULL,
    [Support_id4] int  NULL,
    [Support_id5] int  NULL,
    [Support_id6] int  NULL,
    [Support_id7] int  NULL,
    [Support_id8] int  NULL,
    [Support_id9] int  NULL,
    [Supports_Id] int  NULL
);
GO

-- Creating table 'EscalationChoiceSet'
CREATE TABLE [dbo].[EscalationChoiceSet] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Escalation_id] int  NULL,
    [Escalation_id1] int  NULL,
    [Escalation_id2] int  NULL,
    [Escalation_id3] int  NULL,
    [Escalation_id4] int  NULL,
    [Escalation_id5] int  NULL,
    [Escalation_id6] int  NULL,
    [Escalation_id7] int  NULL,
    [Escalation_id8] int  NULL,
    [Escalation_id9] int  NULL,
    [Escalations_Id] int  NULL
);
GO

-- Creating table 'BusinessBlockChoiceSet'
CREATE TABLE [dbo].[BusinessBlockChoiceSet] (
    [id] int IDENTITY(1,1) NOT NULL,
    [BusinessBlockid] int  NULL,
    [BusinessBlock_id1] int  NULL,
    [BusinessBlock_id2] int  NULL,
    [BusinessBlock_id3] int  NULL,
    [BusinessBlock_id4] int  NULL,
    [BusinessBlock_id5] int  NULL,
    [BusinessBlock_id6] int  NULL,
    [BusinessBlock_id7] int  NULL,
    [BusinessBlock_id8] int  NULL,
    [BusinessBlock_id9] int  NULL,
    [BusinessBlock_Id] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Analytic'
ALTER TABLE [dbo].[Analytic]
ADD CONSTRAINT [PK_Analytic]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'RiskSet'
ALTER TABLE [dbo].[RiskSet]
ADD CONSTRAINT [PK_RiskSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'TimeSheetTable'
ALTER TABLE [dbo].[TimeSheetTable]
ADD CONSTRAINT [PK_TimeSheetTable]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Block'
ALTER TABLE [dbo].[Block]
ADD CONSTRAINT [PK_Block]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'BusinessBlock'
ALTER TABLE [dbo].[BusinessBlock]
ADD CONSTRAINT [PK_BusinessBlock]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClientWays'
ALTER TABLE [dbo].[ClientWays]
ADD CONSTRAINT [PK_ClientWays]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Departments'
ALTER TABLE [dbo].[Departments]
ADD CONSTRAINT [PK_Departments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DirectionsSet'
ALTER TABLE [dbo].[DirectionsSet]
ADD CONSTRAINT [PK_DirectionsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Escalations'
ALTER TABLE [dbo].[Escalations]
ADD CONSTRAINT [PK_Escalations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [State] in table 'ForceQuit'
ALTER TABLE [dbo].[ForceQuit]
ADD CONSTRAINT [PK_ForceQuit]
    PRIMARY KEY CLUSTERED ([State] ASC);
GO

-- Creating primary key on [Id] in table 'Formats'
ALTER TABLE [dbo].[Formats]
ADD CONSTRAINT [PK_Formats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OtdelTable'
ALTER TABLE [dbo].[OtdelTable]
ADD CONSTRAINT [PK_OtdelTable]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Positions'
ALTER TABLE [dbo].[Positions]
ADD CONSTRAINT [PK_Positions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'Process'
ALTER TABLE [dbo].[Process]
ADD CONSTRAINT [PK_Process]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'ProcessType'
ALTER TABLE [dbo].[ProcessType]
ADD CONSTRAINT [PK_ProcessType]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Result'
ALTER TABLE [dbo].[Result]
ADD CONSTRAINT [PK_Result]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'riskChoise'
ALTER TABLE [dbo].[riskChoise]
ADD CONSTRAINT [PK_riskChoise]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'RoleTable'
ALTER TABLE [dbo].[RoleTable]
ADD CONSTRAINT [PK_RoleTable]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SubBlock'
ALTER TABLE [dbo].[SubBlock]
ADD CONSTRAINT [PK_SubBlock]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Supports'
ALTER TABLE [dbo].[Supports]
ADD CONSTRAINT [PK_Supports]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UpravlenieTable'
ALTER TABLE [dbo].[UpravlenieTable]
ADD CONSTRAINT [PK_UpravlenieTable]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'supportChoiceSet'
ALTER TABLE [dbo].[supportChoiceSet]
ADD CONSTRAINT [PK_supportChoiceSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'EscalationChoiceSet'
ALTER TABLE [dbo].[EscalationChoiceSet]
ADD CONSTRAINT [PK_EscalationChoiceSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'BusinessBlockChoiceSet'
ALTER TABLE [dbo].[BusinessBlockChoiceSet]
ADD CONSTRAINT [PK_BusinessBlockChoiceSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [DepartmentsId] in table 'Analytic'
ALTER TABLE [dbo].[Analytic]
ADD CONSTRAINT [FK_DepartmentsAnalytic]
    FOREIGN KEY ([DepartmentsId])
    REFERENCES [dbo].[Departments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentsAnalytic'
CREATE INDEX [IX_FK_DepartmentsAnalytic]
ON [dbo].[Analytic]
    ([DepartmentsId]);
GO

-- Creating foreign key on [DirectionsId] in table 'Analytic'
ALTER TABLE [dbo].[Analytic]
ADD CONSTRAINT [FK_DirectionsAnalytic]
    FOREIGN KEY ([DirectionsId])
    REFERENCES [dbo].[DirectionsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectionsAnalytic'
CREATE INDEX [IX_FK_DirectionsAnalytic]
ON [dbo].[Analytic]
    ([DirectionsId]);
GO

-- Creating foreign key on [UpravlenieTableId] in table 'Analytic'
ALTER TABLE [dbo].[Analytic]
ADD CONSTRAINT [FK_UpravlenieTableAnalytic]
    FOREIGN KEY ([UpravlenieTableId])
    REFERENCES [dbo].[UpravlenieTable]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UpravlenieTableAnalytic'
CREATE INDEX [IX_FK_UpravlenieTableAnalytic]
ON [dbo].[Analytic]
    ([UpravlenieTableId]);
GO

-- Creating foreign key on [OtdelTableId] in table 'Analytic'
ALTER TABLE [dbo].[Analytic]
ADD CONSTRAINT [FK_OtdelTableAnalytic]
    FOREIGN KEY ([OtdelTableId])
    REFERENCES [dbo].[OtdelTable]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OtdelTableAnalytic'
CREATE INDEX [IX_FK_OtdelTableAnalytic]
ON [dbo].[Analytic]
    ([OtdelTableId]);
GO

-- Creating foreign key on [PositionsId] in table 'Analytic'
ALTER TABLE [dbo].[Analytic]
ADD CONSTRAINT [FK_PositionsAnalytic]
    FOREIGN KEY ([PositionsId])
    REFERENCES [dbo].[Positions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PositionsAnalytic'
CREATE INDEX [IX_FK_PositionsAnalytic]
ON [dbo].[Analytic]
    ([PositionsId]);
GO

-- Creating foreign key on [RoleTableId] in table 'Analytic'
ALTER TABLE [dbo].[Analytic]
ADD CONSTRAINT [FK_RoleTableAnalytic]
    FOREIGN KEY ([RoleTableId])
    REFERENCES [dbo].[RoleTable]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleTableAnalytic'
CREATE INDEX [IX_FK_RoleTableAnalytic]
ON [dbo].[Analytic]
    ([RoleTableId]);
GO

-- Creating foreign key on [ClientWaysId] in table 'TimeSheetTable'
ALTER TABLE [dbo].[TimeSheetTable]
ADD CONSTRAINT [FK_ClientWaysTimeSheetTable]
    FOREIGN KEY ([ClientWaysId])
    REFERENCES [dbo].[ClientWays]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientWaysTimeSheetTable'
CREATE INDEX [IX_FK_ClientWaysTimeSheetTable]
ON [dbo].[TimeSheetTable]
    ([ClientWaysId]);
GO

-- Creating foreign key on [FormatsId] in table 'TimeSheetTable'
ALTER TABLE [dbo].[TimeSheetTable]
ADD CONSTRAINT [FK_FormatsTimeSheetTable]
    FOREIGN KEY ([FormatsId])
    REFERENCES [dbo].[Formats]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FormatsTimeSheetTable'
CREATE INDEX [IX_FK_FormatsTimeSheetTable]
ON [dbo].[TimeSheetTable]
    ([FormatsId]);
GO

-- Creating foreign key on [AnalyticId] in table 'TimeSheetTable'
ALTER TABLE [dbo].[TimeSheetTable]
ADD CONSTRAINT [FK_AnalyticTimeSheetTable]
    FOREIGN KEY ([AnalyticId])
    REFERENCES [dbo].[Analytic]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AnalyticTimeSheetTable'
CREATE INDEX [IX_FK_AnalyticTimeSheetTable]
ON [dbo].[TimeSheetTable]
    ([AnalyticId]);
GO

-- Creating foreign key on [Block_id] in table 'Process'
ALTER TABLE [dbo].[Process]
ADD CONSTRAINT [FK_ProcessBlock]
    FOREIGN KEY ([Block_id])
    REFERENCES [dbo].[Block]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProcessBlock'
CREATE INDEX [IX_FK_ProcessBlock]
ON [dbo].[Process]
    ([Block_id]);
GO

-- Creating foreign key on [SubBlockId] in table 'Process'
ALTER TABLE [dbo].[Process]
ADD CONSTRAINT [FK_SubBlockProcess]
    FOREIGN KEY ([SubBlockId])
    REFERENCES [dbo].[SubBlock]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubBlockProcess'
CREATE INDEX [IX_FK_SubBlockProcess]
ON [dbo].[Process]
    ([SubBlockId]);
GO

-- Creating foreign key on [ProcessType_id] in table 'Process'
ALTER TABLE [dbo].[Process]
ADD CONSTRAINT [FK_ProcessTypeProcess]
    FOREIGN KEY ([ProcessType_id])
    REFERENCES [dbo].[ProcessType]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProcessTypeProcess'
CREATE INDEX [IX_FK_ProcessTypeProcess]
ON [dbo].[Process]
    ([ProcessType_id]);
GO

-- Creating foreign key on [Result_id] in table 'Process'
ALTER TABLE [dbo].[Process]
ADD CONSTRAINT [FK_ResultProcess]
    FOREIGN KEY ([Result_id])
    REFERENCES [dbo].[Result]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ResultProcess'
CREATE INDEX [IX_FK_ResultProcess]
ON [dbo].[Process]
    ([Result_id]);
GO

-- Creating foreign key on [Process_id] in table 'TimeSheetTable'
ALTER TABLE [dbo].[TimeSheetTable]
ADD CONSTRAINT [FK_ProcessTimeSheetTable]
    FOREIGN KEY ([Process_id])
    REFERENCES [dbo].[Process]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProcessTimeSheetTable'
CREATE INDEX [IX_FK_ProcessTimeSheetTable]
ON [dbo].[TimeSheetTable]
    ([Process_id]);
GO

-- Creating foreign key on [riskChoise_id] in table 'TimeSheetTable'
ALTER TABLE [dbo].[TimeSheetTable]
ADD CONSTRAINT [FK_riskChoiseTimeSheetTable]
    FOREIGN KEY ([riskChoise_id])
    REFERENCES [dbo].[riskChoise]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_riskChoiseTimeSheetTable'
CREATE INDEX [IX_FK_riskChoiseTimeSheetTable]
ON [dbo].[TimeSheetTable]
    ([riskChoise_id]);
GO

-- Creating foreign key on [Risk_id] in table 'riskChoise'
ALTER TABLE [dbo].[riskChoise]
ADD CONSTRAINT [FK_RiskriskChoise]
    FOREIGN KEY ([Risk_id])
    REFERENCES [dbo].[RiskSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskriskChoise'
CREATE INDEX [IX_FK_RiskriskChoise]
ON [dbo].[riskChoise]
    ([Risk_id]);
GO

-- Creating foreign key on [supportChoice_id] in table 'TimeSheetTable'
ALTER TABLE [dbo].[TimeSheetTable]
ADD CONSTRAINT [FK_TimeSheetTablesupportChoice]
    FOREIGN KEY ([supportChoice_id])
    REFERENCES [dbo].[supportChoiceSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TimeSheetTablesupportChoice'
CREATE INDEX [IX_FK_TimeSheetTablesupportChoice]
ON [dbo].[TimeSheetTable]
    ([supportChoice_id]);
GO

-- Creating foreign key on [Supports_Id] in table 'supportChoiceSet'
ALTER TABLE [dbo].[supportChoiceSet]
ADD CONSTRAINT [FK_supportChoiceSupports]
    FOREIGN KEY ([Supports_Id])
    REFERENCES [dbo].[Supports]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_supportChoiceSupports'
CREATE INDEX [IX_FK_supportChoiceSupports]
ON [dbo].[supportChoiceSet]
    ([Supports_Id]);
GO

-- Creating foreign key on [Escalations_Id] in table 'EscalationChoiceSet'
ALTER TABLE [dbo].[EscalationChoiceSet]
ADD CONSTRAINT [FK_EscalationChoiceEscalations]
    FOREIGN KEY ([Escalations_Id])
    REFERENCES [dbo].[Escalations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EscalationChoiceEscalations'
CREATE INDEX [IX_FK_EscalationChoiceEscalations]
ON [dbo].[EscalationChoiceSet]
    ([Escalations_Id]);
GO

-- Creating foreign key on [EscalationChoice_id] in table 'TimeSheetTable'
ALTER TABLE [dbo].[TimeSheetTable]
ADD CONSTRAINT [FK_TimeSheetTableEscalationChoice]
    FOREIGN KEY ([EscalationChoice_id])
    REFERENCES [dbo].[EscalationChoiceSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TimeSheetTableEscalationChoice'
CREATE INDEX [IX_FK_TimeSheetTableEscalationChoice]
ON [dbo].[TimeSheetTable]
    ([EscalationChoice_id]);
GO

-- Creating foreign key on [BusinessBlock_Id] in table 'BusinessBlockChoiceSet'
ALTER TABLE [dbo].[BusinessBlockChoiceSet]
ADD CONSTRAINT [FK_BusinessBlockBusinessBlockChoice]
    FOREIGN KEY ([BusinessBlock_Id])
    REFERENCES [dbo].[BusinessBlock]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BusinessBlockBusinessBlockChoice'
CREATE INDEX [IX_FK_BusinessBlockBusinessBlockChoice]
ON [dbo].[BusinessBlockChoiceSet]
    ([BusinessBlock_Id]);
GO

-- Creating foreign key on [BusinessBlockChoice_id] in table 'TimeSheetTable'
ALTER TABLE [dbo].[TimeSheetTable]
ADD CONSTRAINT [FK_TimeSheetTableBusinessBlockChoice]
    FOREIGN KEY ([BusinessBlockChoice_id])
    REFERENCES [dbo].[BusinessBlockChoiceSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TimeSheetTableBusinessBlockChoice'
CREATE INDEX [IX_FK_TimeSheetTableBusinessBlockChoice]
ON [dbo].[TimeSheetTable]
    ([BusinessBlockChoice_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------