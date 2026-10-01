using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace studySpaceWebApp.Models;

public partial class StudySpaceDbContext : DbContext
{
    public StudySpaceDbContext()
    {
    }

    public StudySpaceDbContext(DbContextOptions<StudySpaceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuthProvider> AuthProviders { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<NotesHistory> NotesHistories { get; set; }

    public virtual DbSet<Poll> Polls { get; set; }

    public virtual DbSet<PollOption> PollOptions { get; set; }

    public virtual DbSet<PollVote> PollVotes { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<RoomMember> RoomMembers { get; set; }

    public virtual DbSet<StudyRoom> StudyRooms { get; set; }

    public virtual DbSet<ToDoItem> ToDoItems { get; set; }

    public virtual DbSet<User> Users { get; set; }
   

    public DbSet<Admin> Admin { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=studySpaceDb;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthProvider>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("PK__AuthProv__B54C687D1213153D");

            entity.HasOne(d => d.User).WithMany(p => p.AuthProviders).HasConstraintName("FK__AuthProvi__UserI__3C69FB99");
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__ChatMess__C87C0C9C129DADE8");

            entity.Property(e => e.SentAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Room).WithMany(p => p.ChatMessages)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ChatMessa__RoomI__48CFD27E");

            entity.HasOne(d => d.User).WithMany(p => p.ChatMessages).HasConstraintName("FK__ChatMessa__UserI__49C3F6B7");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("PK__Notes__EACE355FA53DCEC2");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Notes).HasConstraintName("FK__Notes__CreatedBy__52593CB8");

            entity.HasOne(d => d.Room).WithMany(p => p.Notes)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Notes__RoomId__5165187F");
        });

        modelBuilder.Entity<NotesHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__NotesHis__4D7B4ABD73031E9C");

            entity.Property(e => e.EditTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.EditedByNavigation).WithMany(p => p.NotesHistories).HasConstraintName("FK__NotesHist__Edite__571DF1D5");

            entity.HasOne(d => d.Note).WithMany(p => p.NotesHistories)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__NotesHist__NoteI__5629CD9C");
        });

        modelBuilder.Entity<Poll>(entity =>
        {
            entity.HasKey(e => e.PollId).HasName("PK__Polls__E1949E6AABC9D698");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Polls).HasConstraintName("FK__Polls__CreatedBy__5BE2A6F2");

            entity.HasOne(d => d.Room).WithMany(p => p.Polls)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Polls__RoomId__5AEE82B9");
        });

        modelBuilder.Entity<PollOption>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__PollOpti__92C7A1FF5AFB4B41");

            entity.HasOne(d => d.Poll).WithMany(p => p.PollOptions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__PollOptio__PollI__5FB337D6");
        });

        modelBuilder.Entity<PollVote>(entity =>
        {
            entity.HasKey(e => e.VoteId).HasName("PK__PollVote__52F015C25AF492A3");

            entity.Property(e => e.VotedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Option).WithMany(p => p.PollVotes).HasConstraintName("FK__PollVotes__Optio__628FA481");

            entity.HasOne(d => d.User).WithMany(p => p.PollVotes).HasConstraintName("FK__PollVotes__UserI__6383C8BA");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("PK__Resource__4ED1816F6305EA3A");

            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.Resources).HasConstraintName("FK__Resources__Uploa__4D94879B");
        });

        modelBuilder.Entity<RoomMember>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__RoomMemb__0CF04B18FBB44F06");

            entity.Property(e => e.JoinedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomMembers)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__RoomMembe__RoomI__440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.RoomMembers)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__RoomMembe__UserI__44FF419A");
        });

        modelBuilder.Entity<StudyRoom>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__StudyRoo__32863939346870E0");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsPrivate).HasDefaultValue(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StudyRooms).HasConstraintName("FK__StudyRoom__Creat__403A8C7D");
        });

        modelBuilder.Entity<ToDoItem>(entity =>
        {
            entity.HasKey(e => e.ToDoId).HasName("PK__ToDoItem__21D08D00FE635203");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);

            entity.HasOne(d => d.Room).WithMany(p => p.ToDoItems)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ToDoItems__RoomI__6754599E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CAC654755");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Role).HasDefaultValue("Student");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
