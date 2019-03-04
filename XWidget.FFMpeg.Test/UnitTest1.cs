using System;
using Xunit;

namespace XWidget.FFMpeg.Test {
    public class UnitTest1 {
        [Fact]
        public void Test1() {
            var builder = new FFMpegConverterBuilder();
            builder
                .ConfigGeneric(option => //���L40���10��
                    option.SetStartPosition(40).SetDuration(10)
                )
                .ConfigVideo(option => //�]�w�s�ؤo�B�ഫ�t�׳]���D�`�֡A�վ�~�謰23
                    option.SetSize(352, 240).SetPreset(VideoOption.Preset.veryfast).SetCrf(23)
                )
                .ConfigAudio(option => //�]�w�n�����˲v��16K�B�����n�D�A��S�v��32K
                    option.SetChannels(1).SetFrequency(16 * 1000).SetBitrate(32 * 1000)
                );

            //var command = builder.CreateCommand(new string[] { "input.mp4" }, "output.mp4");
        }

        [Fact]
        public void Test2() {
            var builder = new FFMpegConverterBuilder();
            builder
                .ConfigGeneric(option => //���L40���10��
                    option.SetStartPosition(40).SetDuration(10)
                )
                .ConfigVideo(option => //�h���v��
                    option.RemoveVideo()
                )
                .ConfigAudio(option => //�]�w�n�����˲v��16K�B�����n�D�A��S�v��32K
                    option.SetChannels(1).SetFrequency(16 * 1000).SetBitrate(32 * 1000)
                );

            //var kk = builder.CreateCommand(new string[] { "input.mp4" }, "output.mp3");
        }
    }
}
