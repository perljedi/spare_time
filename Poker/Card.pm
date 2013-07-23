package Poker::Card;
use base qw(Exporter);
use vars qw(@EXPORT @EXPORT_OK %EXPORT_TAGS %suites %cards);

my(%objects)=();

BEGIN{
   
}

sub new
{
	my $proto = shift;
	$proto = ref($proto) || $proto;
	my($value, $suite) = @_;
   my $orig='card';
   my $self = bless(\$orig,$proto);

	$objects{$self} = {suite=>$suite, value=>$value};
   return $self;
}

sub getSuite
{
	my $self = shift;
	return $objects{$self}{suite};
}

sub getValue
{
	my $self=shift;
	return $objects{$self}{value};
}

sub getColor
{
   my $self = shift;
   if($objects{$self}{suite} eq 'd' || $objects{$self}{suite} eq 'h'){
      return 'red';
   }else{
      return 'black';
   }
}

sub isRed
{
   my $self = shift;
   return $self->getColor() eq 'red';
}

sub isBlack
{
   my $self = shift;
   return !$self->isRed();
}

return 42;
