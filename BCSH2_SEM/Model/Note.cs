﻿using SQLite;
namespace BCSH2_SEM.Model;

public class Note
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [Indexed]
    public int NotebookId { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string FileLocation { get; set; }
}