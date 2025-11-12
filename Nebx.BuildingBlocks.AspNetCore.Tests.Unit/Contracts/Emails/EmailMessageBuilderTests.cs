using Nebx.BuildingBlocks.AspNetCore.Contracts.Emails;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Unit.Contracts.Emails;

public class EmailMessageBuilderTests
    {
        private static readonly EmailAddress Sender = new("Admin", "admin@example.com");
        private static readonly EmailAddress Recipient = new("User", "user@example.com");
        private static readonly EmailAddress CcRecipient = new("CC", "cc@example.com");
        private static readonly EmailAddress BccRecipient = new("BCC", "bcc@example.com");

        private static readonly EmailAttachment Attachment =
            new("file.txt", new byte[] { 1, 2, 3 }, "text/plain");

        // --------------------------------------------------------------------
        // Success Cases
        // --------------------------------------------------------------------

        [Fact]
        public void Build_ShouldReturnConfiguredMessage_WhenValid()
        {
            // Arrange
            var builder = new EmailMessageBuilder()
                .From(Sender)
                .To([Recipient])
                .Subject("Greetings")
                .Body("Hello there", isHtml: false)
                .Attachment([Attachment]);

            // Act
            var message = builder.Build();

            // Assert
            Assert.Equal(Sender, message.From);
            Assert.Contains(Recipient, message.To);
            Assert.Equal("Greetings", message.Subject);
            Assert.Equal("Hello there", message.Body);
            Assert.False(message.IsHtml);
            Assert.Contains(Attachment, message.Attachments);
        }

        [Fact]
        public void ShouldSetCcAndBccRecipients_WhenConfigured()
        {
            // Arrange
            var builder = new EmailMessageBuilder()
                .To([Recipient])
                .Subject("Test")
                .Cc([CcRecipient])
                .Bcc([BccRecipient]);

            // Act
            var message = builder.Build();

            // Assert
            Assert.Contains(CcRecipient, message.Cc);
            Assert.Contains(BccRecipient, message.Bcc);
        }

        [Fact]
        public void ShouldSupportFluentChaining()
        {
            // Arrange & Act
            var message = new EmailMessageBuilder()
                .From(Sender)
                .To([Recipient])
                .Subject("Test")
                .Body("Body")
                .Build();

            // Assert
            Assert.Equal(Sender, message.From);
            Assert.Contains(Recipient, message.To);
            Assert.Equal("Test", message.Subject);
            Assert.Equal("Body", message.Body);
        }

        [Fact]
        public void ShouldRemoveDuplicateRecipients()
        {
            // Arrange
            var recipients = new[]
            {
                new EmailAddress("User", "user@example.com"),
                new EmailAddress("USER", "user@example.com") // duplicate (case-insensitive)
            };

            var builder = new EmailMessageBuilder()
                .To(recipients)
                .Subject("Subject");

            // Act
            var message = builder.Build();

            // Assert
            Assert.Single(message.To);
        }

        [Fact]
        public void ShouldRemoveDuplicateAttachments()
        {
            // Arrange
            var attachments = new[]
            {
                new EmailAttachment("file.txt", [1, 2, 3], "text/plain"),
                new EmailAttachment("file.txt", [1, 2, 3], "text/plain") // duplicate
            };

            var builder = new EmailMessageBuilder()
                .To([Recipient])
                .Subject("Test")
                .Attachment(attachments);

            // Act
            var message = builder.Build();

            // Assert
            Assert.Single(message.Attachments);
        }

        [Fact]
        public void Builder_ShouldBeReusableIndependently()
        {
            // Arrange
            var builderA = new EmailMessageBuilder()
                .To([Recipient])
                .Subject("Message A");

            var builderB = new EmailMessageBuilder()
                .To([Recipient])
                .Subject("Message B");

            // Act
            var msgA = builderA.Build();
            var msgB = builderB.Build();

            // Assert
            Assert.NotSame(msgA, msgB);
            Assert.NotEqual(msgA.Subject, msgB.Subject);
        }

        // --------------------------------------------------------------------
        // Validation & Error Cases
        // --------------------------------------------------------------------

        [Fact]
        public void Build_ShouldThrow_WhenNoRecipients()
        {
            // Arrange
            var builder = new EmailMessageBuilder()
                .Subject("Missing recipients");

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => builder.Build());
            Assert.Equal("Email address must have at least one recipient", ex.Message);
        }

        [Fact]
        public void Build_ShouldThrow_WhenSubjectIsEmpty()
        {
            // Arrange
            var builder = new EmailMessageBuilder()
                .To([Recipient]);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => builder.Build());
            Assert.Equal("Subject must have a value", ex.Message);
        }

        [Fact]
        public void Body_ShouldSetIsHtmlFlagCorrectly()
        {
            // Arrange
            var builder = new EmailMessageBuilder()
                .To([Recipient])
                .Subject("Body Test")
                .Body("Plain text", isHtml: false);

            // Act
            var message = builder.Build();

            // Assert
            Assert.False(message.IsHtml);
            Assert.Equal("Plain text", message.Body);
        }

        [Fact]
        public void From_ShouldReplaceExistingSender()
        {
            // Arrange
            var oldSender = new EmailAddress("Old", "old@example.com");
            var newSender = new EmailAddress("New", "new@example.com");

            var builder = new EmailMessageBuilder()
                .From(oldSender)
                .To([Recipient])
                .Subject("Change Sender")
                .From(newSender);

            // Act
            var message = builder.Build();

            // Assert
            Assert.Equal(newSender, message.From);
            Assert.NotEqual(oldSender, message.From);
        }

        [Fact]
        public void Attachment_ShouldReplaceExistingAttachments()
        {
            // Arrange
            var firstAttachment = new EmailAttachment("a.txt", [1], "text/plain");
            var secondAttachment = new EmailAttachment("b.txt", [2], "text/plain");

            var builder = new EmailMessageBuilder()
                .To([Recipient])
                .Subject("Replace Attachments")
                .Attachment([firstAttachment])
                .Attachment([secondAttachment]); // should replace the first set

            // Act
            var message = builder.Build();

            // Assert
            Assert.DoesNotContain(firstAttachment, message.Attachments);
            Assert.Contains(secondAttachment, message.Attachments);
            Assert.Single(message.Attachments);
        }
    }